using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using IdentityServer4.Endpoints.Results;
using Lean.Authorization;
using Lean.Lessons.Dto;
using Lean.Lessons.Dto.Import;
using Lean.Lessons.Importing;
using Lean.Options;
using Lean.UserLessonsProgress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    [AbpAuthorize(AppPermissions.Pages_Tenant_Lessons)]
    public class LessonsAppService : LeanAppServiceBase, ILessonsAppService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Lesson> _lessonRepository;
        private readonly IRepository<Problem> _problemRepository;
        private readonly IRepository<UserLearningProgress> _userLearningProgressRepository;
        private readonly IRepository<UserLessonAnswerSet> _userLessonAnswerSetRepository;
        private readonly IRepository<UserProblemAnswerResult> _userProblemResultRepository;
        private readonly IRepository<UserTagRating, string> _userTagRatingRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<FlowRule> _flowRuleRepository;
        private readonly ICourseExcelImporter _courseExcelImporter;
        private readonly RatingVariables _ratingVariables;

        public LessonsAppService(
            IRepository<Course> courseRepository,
            IRepository<Module> moduleRepository,
            IRepository<Lesson> lessonRepository,
            IRepository<Problem> problemRepository,
            IRepository<UserLearningProgress> userLearningProgressRepository,
            IRepository<UserLessonAnswerSet> userLessonAnswerSetRepository,
            IRepository<UserProblemAnswerResult> userProblemResultRepository,
            IRepository<UserTagRating, string> userTagRatingRepository,
            IRepository<Tag> tagRepository,
            IRepository<FlowRule> flowRuleRepository,
            IOptions<RatingVariables> ratingVariables,
            ICourseExcelImporter courseExcelImporter)
        {
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
            _lessonRepository = lessonRepository;
            _problemRepository = problemRepository;
            _userLearningProgressRepository = userLearningProgressRepository;
            _userLessonAnswerSetRepository = userLessonAnswerSetRepository;
            _userProblemResultRepository = userProblemResultRepository;
            _userTagRatingRepository = userTagRatingRepository;
            _tagRepository = tagRepository;
            _flowRuleRepository = flowRuleRepository;
            _courseExcelImporter = courseExcelImporter;
            _ratingVariables = ratingVariables.Value;
        }

        public async Task<CurrentLessonDto> GetCurrentLesson()
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            var currentLesson = await GetCurrentLessonDto(learningProgress);
            return currentLesson;
        }

        public async Task<CurrentLessonDto> MoveToNextStep()
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            await TransferProgressToNextStep(learningProgress);

            var currentLesson = await GetCurrentLessonDto(learningProgress);
            return currentLesson;
        }

        public async Task<CurrentLessonDto> MoveToPreviousStep()
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            learningProgress.Step = GetPreviousStep(learningProgress.Step);

            var currentLesson = await GetCurrentLessonDto(learningProgress);
            return currentLesson;
        }

        public async Task<CurrentLessonDto> SubmitProblemAnswer(SubmitProblemAnswerInput input)
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            if (learningProgress.CurrentProblemId != input.ProblemId)
            {
                throw new UserFriendlyException("Something went wrong");
            }

            var problem = await _problemRepository.GetAll()
                .Include(x => x.ProblemAnswerOptions)
                .Include(x => x.ProblemTags).ThenInclude(x => x.TagFk)
                .Where(x => x.Id == input.ProblemId)
                .FirstOrDefaultAsync();

            var isAnswerCorrect = false;
            var correctAnswerText = string.Empty;
            var answerSet = await GetCurrentUserLessonAnswerSet(learningProgress.CurrentLessonId);
            if (problem.Type == ProblemType.FreeText)
            {
                var textAnswer = input.FreeTextAnswer.Trim();
                var correctAnswer = problem.ProblemAnswerOptions.First(x => x.IsCorrect);
                correctAnswerText = correctAnswer.Text;
                isAnswerCorrect = IsAnswerCorrect(correctAnswerText, textAnswer);
                var problemResult = new UserProblemAnswerResult
                {
                    IsCorrect = isAnswerCorrect,
                    TextAnswer = textAnswer,
                    ProblemId = problem.Id,
                    UserLessonAnswerSetFk = answerSet

                };
                await _userProblemResultRepository.InsertAsync(problemResult);
                await UpdateUserTagRatings(isAnswerCorrect, problem.ProblemTags);
                var nextLessonId = await GetNextLessonProblemId(learningProgress);
                learningProgress.CurrentProblemId = nextLessonId;
                if (nextLessonId is null)
                {
                    await TransferProgressToNextStep(learningProgress);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            var currentLesson = await GetCurrentLessonDto(learningProgress);
            currentLesson.IsPreviousAnswerCorrect = isAnswerCorrect;
            currentLesson.CorrectAnswer = correctAnswerText;
            return currentLesson;
        }

        private bool IsAnswerCorrect(string correctAnswer, string answer)
        {
            correctAnswer = correctAnswer.Trim();
            answer = answer?.Trim();
            var isSampleNumber = Decimal.TryParse(correctAnswer, out var correctNumber);
            var isAnswerNumber = Decimal.TryParse(answer, out var number);
            if (isSampleNumber && isAnswerNumber)
            {
                return correctNumber == number;
            }

            return correctAnswer.EqualsIgnoreCase(answer);
        }

        private async Task<UserLessonAnswerSet> GetCurrentUserLessonAnswerSet(int lessonId)
        {
            var mostRecentAnswerSet = await _userLessonAnswerSetRepository.GetAll()
                .Include(x => x.Answers)
                .Where(x => x.UserId == AbpSession.UserId.Value && x.LessonId == lessonId)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefaultAsync();

            if (mostRecentAnswerSet is null)
            {
                mostRecentAnswerSet = CreateUserLessonAnswerSet(lessonId);
                await _userLessonAnswerSetRepository.InsertAsync(mostRecentAnswerSet);
            }
            else
            {
                var counts = await _userLessonAnswerSetRepository.GetAll()
                    .Where(x => x.Id == mostRecentAnswerSet.Id)
                    .Select(x => new
                    {
                        ProblemsCount = x.LessonFk.Problems.Count(),
                        AnswersCount = x.Answers.Count()
                    })
                    .FirstOrDefaultAsync();
                if (counts.AnswersCount == counts.ProblemsCount)
                {
                    mostRecentAnswerSet = CreateUserLessonAnswerSet(lessonId);
                    await _userLessonAnswerSetRepository.InsertAsync(mostRecentAnswerSet);
                }
            }

            return mostRecentAnswerSet;
        }

        private UserLessonAnswerSet CreateUserLessonAnswerSet(int lessonId)
        {
            return new UserLessonAnswerSet
            {
                UserId = AbpSession.UserId.Value,
                LessonId = lessonId,
                Answers = new List<UserProblemAnswerResult>()
            };
        }

        private async Task UpdateUserTagRatings(bool isAnswerCorrect, IEnumerable<ProblemTag> problemTags)
        {
            foreach (var problemTag in problemTags)
            {
                var tagRating = await GetOrCreateUserTagRating(problemTag);
                var newRatingScore = CalculateNewRating(
                    tagRating.Rating,
                    problemTag.ProblemTagRating,
                    _ratingVariables.T,
                    _ratingVariables.K,
                    _ratingVariables.C,
                    isAnswerCorrect);
                tagRating.Rating = newRatingScore;
            }
        }

        private static decimal CalculateNewRating(decimal old, decimal tagValue, decimal t, decimal k, decimal c, bool successful)
        {
            decimal Pow(decimal x, decimal y) => (decimal)Math.Pow((double)x, (double)y);

            var n = successful ? 1m : 0m;
            var newRating = old + c * (n - 1m / (1m + (1m / t - 1m) * Pow(10m, (tagValue - old) / k)));
            return newRating;
        }

        private async Task<UserTagRating> GetOrCreateUserTagRating(ProblemTag problemTag)
        {
            var tagRating = await _userTagRatingRepository.GetAll()
                .Where(x => x.UserId == AbpSession.UserId.Value && x.TagId == problemTag.TagId)
                .FirstOrDefaultAsync();
            if (tagRating is null)
            {
                tagRating = new UserTagRating
                {
                    TagId = problemTag.TagId,
                    UserId = AbpSession.UserId.Value,
                    Rating = problemTag.TagFk.InitialRating
                };
                await _userTagRatingRepository.InsertAsync(tagRating);
            }

            return tagRating;
        }

        private async Task<UserLearningProgress> GetCurrentUserLearningProgress()
        {
            var learningProgress = await _userLearningProgressRepository.GetAll()
                .Include(x => x.CurrentLessonFk)
                .FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId.Value);
            if (learningProgress is null)
            {
                var firstLessonId = await _lessonRepository.GetAll()
                    .Where(x => x.IsInitial)
                    .OrderBy(x => x.ModuleFk.Priority)
                    .Select(x => x.Id).FirstOrDefaultAsync();
                learningProgress = new UserLearningProgress
                {
                    UserId = AbpSession.UserId.Value,
                    Step = LessonStep.Score,
                    CurrentLessonId = firstLessonId,
                };
                await _userLearningProgressRepository.InsertAsync(learningProgress);
                await CurrentUnitOfWork.SaveChangesAsync();
                learningProgress = await _userLearningProgressRepository.GetAll()
                    .Include(x => x.CurrentLessonFk)
                    .FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId.Value);
            }

            return learningProgress;
        }

        private async Task TransferProgressToNextStep(UserLearningProgress learningProgress)
        {
            var nextStep = GetNextStep(learningProgress.Step);
            learningProgress.Step = nextStep;

            if (nextStep == LessonStep.Lesson)
            {
                await MoveToNextLesson(learningProgress);
            }
        }

        private async Task<CurrentLessonDto> GetCurrentLessonDto(UserLearningProgress learningProgress)
        {
            return new CurrentLessonDto
            {
                Lesson = await GetLesson(learningProgress.CurrentLessonId),
                Problem = await GetProblem(learningProgress),
                Step = learningProgress.Step,
                TagScores = await GetTagScores(learningProgress)
            };
        }

        private async Task MoveToNextLesson(UserLearningProgress learningProgress)
        {
            // TODO: Implement moving to new lessons.
            var lessonId = learningProgress.CurrentLessonId;
            var mostRecentAnswerSet = await _userLessonAnswerSetRepository.GetAll().AsNoTracking()
                .Include(x => x.Answers)
                .Where(x => x.UserId == AbpSession.UserId.Value
                            && x.LessonId == lessonId
                            && x.Answers.Count == x.LessonFk.Problems.Count)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefaultAsync();
            if (mostRecentAnswerSet is null)
            {
                return; // Very first lesson.
            }

            var flowRules = await _flowRuleRepository.GetAll().AsNoTracking()
                .Include(x => x.FlowRuleNextLessons)
                .Include(x => x.FlowRuleProblems)
                .Where(x => x.LessonId == lessonId)
                .OrderBy(x => x.Priority)
                .ToListAsync();
            var answersMap = mostRecentAnswerSet.Answers.ToDictionary(x => x.ProblemId, x => x.IsCorrect);
            foreach (var rule in flowRules)
            {
                var correctAnswersCount = rule.FlowRuleProblems.Count(x => answersMap[x.ProblemId]);
                var conditionSatisfied = rule.Condition switch 
                { 
                    FlowCondition.LessThan => correctAnswersCount < rule.CorrectAnswersCount,
                    FlowCondition.MoreThan => correctAnswersCount > rule.CorrectAnswersCount,
                    FlowCondition.Default => true,
                    _ => false // Something went wrong
                };
                if (conditionSatisfied)
                {
                    var nextLessonIdsOrdered = rule.FlowRuleNextLessons?.OrderBy(x => x.Priority).Select(x => x.NextLessonId).ToList();
                    if (nextLessonIdsOrdered is null || nextLessonIdsOrdered.Count == 0)
                    {
                        learningProgress.Step = LessonStep.MvpCompleted;
                        return;
                    }
                    var answerSets = await _userLessonAnswerSetRepository.GetAll().AsNoTracking()
                        .Where(x => nextLessonIdsOrdered.Contains(x.LessonId))
                        .ToListAsync();
                    var nextLessonId = nextLessonIdsOrdered
                        .Select(x => (int?)x)
                        .FirstOrDefault(x => !answerSets.Any(a => a.LessonId == x)) 
                        ?? nextLessonIdsOrdered.First();
                    learningProgress.CurrentLessonId = nextLessonId;
                    return;
                }
            }
        }

        private async Task<LessonDto> GetLesson(int currentLessonId)
        {
            var query = _lessonRepository.GetAll().Where(x => x.Id == currentLessonId);
            var mappedQuery = ObjectMapper.ProjectTo<LessonDto>(query);
            var lesson = await mappedQuery.FirstOrDefaultAsync();
            return lesson;
        }

        private async Task<List<TagScoreDto>> GetTagScores(UserLearningProgress learningProgress)
        {
            if (learningProgress.Step != LessonStep.Score || learningProgress.Step != LessonStep.MvpCompleted)
            {
                return null;
            }

            var moduleId = learningProgress.CurrentLessonFk.ModuleId;
            var moduleTags = await _tagRepository.GetAll().AsNoTracking()
                //.Where(x => x.ModuleId == moduleId)
                .ToListAsync();

            var userTagRatings = await _userTagRatingRepository.GetAll().AsNoTracking()
                .Where(x => x.UserId == AbpSession.UserId.Value && x.TagFk.ModuleId == moduleId)
                .ToListAsync();
            var userTagRatingsTagMap = userTagRatings.ToDictionary(x => x.TagId, x => x.Rating);

            var tagScores = moduleTags
                .Select(x =>
                {
                    return new TagScoreDto
                    {
                        Rating = userTagRatingsTagMap.GetValueOrDefault(x.Id, x.InitialRating),
                        TagName = x.Name
                    };
                })
                .OrderBy(x => x.TagName)
                .ToList();
            return tagScores;
        }

        private async Task<ProblemDto> GetProblem(UserLearningProgress learningProgress)
        {
            if (learningProgress.Step == LessonStep.ProblemSet)
            {
                if (learningProgress.CurrentProblemId is null)
                {
                    var nextProblemId = await GetNextLessonProblemId(learningProgress);
                    learningProgress.CurrentProblemId = nextProblemId;
                }

                var problemQuery = _problemRepository.GetAll().Where(x => x.Id == learningProgress.CurrentProblemId);
                var problem = await ObjectMapper.ProjectTo<ProblemDto>(problemQuery).FirstOrDefaultAsync();
                return problem;
            }

            return null;
        }

        private async Task<int?> GetNextLessonProblemId(UserLearningProgress learningProgress)
        {
            var baseQuery = _problemRepository.GetAll()
                .Where(x => x.LessonId == learningProgress.CurrentLessonId)
                .OrderBy(x => x.Id);
            if (learningProgress.CurrentProblemId.HasValue)
            {
                var id = await baseQuery
                    .Where(x => x.Id > learningProgress.CurrentProblemId)
                    .Select<Problem, int?>(x => x.Id)
                    .FirstOrDefaultAsync();
                return id;
            }
            var firstId = await baseQuery.Select<Problem, int?>(x => x.Id)
                    .FirstOrDefaultAsync();
            return firstId;
        }

        private LessonStep GetNextStep(LessonStep step) => step switch
        {
            LessonStep.Lesson => LessonStep.Activity,
            LessonStep.Activity => LessonStep.ProblemSet,
            LessonStep.ProblemSet => LessonStep.Score,
            LessonStep.Score => LessonStep.Lesson,
            LessonStep.MvpCompleted => LessonStep.MvpCompleted, // No escape (:
            _ => throw new NotImplementedException()
        };

        private LessonStep GetPreviousStep(LessonStep step) => step switch
        {
            LessonStep.Activity => LessonStep.Lesson,
            LessonStep.ProblemSet => LessonStep.ProblemSet, // Can't move back from problem set
            LessonStep.Score => LessonStep.Score, // Can't move back from score page
            LessonStep.Lesson => LessonStep.Score,
            LessonStep.MvpCompleted => LessonStep.MvpCompleted,
            _ => throw new NotImplementedException()
        };

        #region Import
        [UnitOfWork]
        public async Task ImportCouserFromExcel([FromForm] IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var course = _courseExcelImporter.Import(stream, Path.GetFileNameWithoutExtension(file.FileName));
            await _courseRepository.InsertAsync(course);
            //return await ImportRecipesExcel(file, false);
        }

        [UnitOfWork]
        public async Task ImportCourses(List<CourseImportDto> courseDtos)
        {
            foreach (var courseDto in courseDtos)
            {
                var course = new Course
                {
                    Name = courseDto.Name,
                    Description = courseDto.Description
                };

                //var tags = courseDto.Modules.ToDictionary(x => x.Key, x => new m)
            }

            var courses = courseDtos.Select(courseDto =>
            {
                var course = new Course
                {
                    Name = courseDto.Name,
                    Description = courseDto.Description
                };

                var modules = courseDto.Modules.Select(moduleDto =>
                {
                    var module = new Module { Name = moduleDto.Key };
                    var tagDict = moduleDto.Tags.ToDictionary(x => x.Key, x => new Tag
                    {
                        InitialRating = x.InitialRating,
                        Name = x.Name
                    });
                    module.Tags = tagDict.Select(x => x.Value).ToList();
                    var lessons = moduleDto.Lessons.Select(lessonDto =>
                    {
                        var lesson = new Lesson
                        {
                            ActivityText = lessonDto.ActivityText,
                            ActivityVideoHtml = lessonDto.ActivityVideoHtml,
                            LessonText = lessonDto.LessonText,
                            LessonVideoHtml = lessonDto.LessonVideoHtml,
                            IsInitial = lessonDto.IsInitial,
                            Name = lessonDto.Name
                        };
                        lesson.Problems = lessonDto.Problems.Select(problemDto =>
                        {
                            var problem = ObjectMapper.Map<Problem>(problemDto);
                            problem.ProblemAnswerOptions = problemDto.ProblemAnswerOptions
                                .Select(optionDto => ObjectMapper.Map<ProblemAnswerOption>(optionDto))
                                .ToList();
                            problem.ProblemTags = problemDto.TagRef.Select(tag => new ProblemTag
                            {
                                TagFk = tagDict[tag.TagKey],
                                ProblemTagRating = tag.Rating
                            }).ToList();
                            return problem;
                        }).ToList();
                        return lesson;
                    }).ToList();
                    module.Lessons = lessons;
                    return module;
                }).ToList();
                course.Modules = modules;
                return course;
            }).ToList();

            foreach (var course in courses)
            {
                await _courseRepository.InsertAsync(course);
            }
        }
        #endregion
    }
}
