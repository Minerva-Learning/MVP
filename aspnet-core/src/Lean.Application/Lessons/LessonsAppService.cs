using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Lean.Lessons.Dto;
using Lean.UserLessonsProgress;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    [AbpAuthorize]
    public class LessonsAppService : LeanAppServiceBase, ILessonsAppService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Lesson> _lessonRepository;
        private readonly IRepository<Problem> _problemRepository;
        private readonly IRepository<UserLearningProgress> _userLearningProgressRepository;
        private readonly IRepository<UserProblemResult> _userProblemResultRepository;

        public LessonsAppService(
            IRepository<Course> courseRepository,
            IRepository<Module> moduleRepository,
            IRepository<Lesson> lessonRepository,
            IRepository<Problem> problemRepository,
            IRepository<UserLearningProgress> userLearningProgressRepository,
            IRepository<UserProblemResult> userProblemResultRepository)
        {
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
            _lessonRepository = lessonRepository;
            _problemRepository = problemRepository;
            _userLearningProgressRepository = userLearningProgressRepository;
            _userProblemResultRepository = userProblemResultRepository;
        }

        public async Task<CurrentLessonDto> GetCurrentLesson()
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            var currentLesson = new CurrentLessonDto
            {
                Lesson = await GetLesson(learningProgress.CurrentLessonId),
                Problem = await GetProblem(learningProgress),
                Step = learningProgress.Step
            };

            return currentLesson;
        }

        public async Task<CurrentLessonDto> MoveToNextStep()
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            var nextStep = GetNextStep(learningProgress.Step);
            learningProgress.Step = nextStep;

            if (nextStep == LessonStep.Lesson)
            {
                await MoveToNewLesson(learningProgress);
                // Moving to the next lesson logic.
            }

            var currentLesson = new CurrentLessonDto
            {
                Lesson = await GetLesson(learningProgress.CurrentLessonId),
                Problem = await GetProblem(learningProgress),
                Step = nextStep
            };
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
                .Where(x => x.Id == input.ProblemId)
                .FirstOrDefaultAsync();

            if (problem.Type == ProblemType.FreeText)
            {
                var textAnswer = input.FreeTextAnswer.Trim();
                var correctAnswer = problem.ProblemAnswerOptions.First(x => x.IsCorrect);
                var correct = correctAnswer.Text == textAnswer;
                var problemResult = new UserProblemResult
                {
                    IsCorrect = correct,
                    TextAnswer = textAnswer,
                    UserId = AbpSession.UserId.Value,
                    ProblemId = problem.Id,
                };
                await _userProblemResultRepository.InsertAsync(problemResult);
                var nextLessonId = await GetNextLessonProblemId(learningProgress);
                learningProgress.CurrentProblemId = nextLessonId;
            }
            else
            {
                throw new NotImplementedException();
            }

            var currentLesson = new CurrentLessonDto
            {
                Lesson = await GetLesson(learningProgress.CurrentLessonId),
                Problem = await GetProblem(learningProgress),
                Step = learningProgress.Step
            };

            return currentLesson;
        }

        [UnitOfWork]
        public async Task UploadCourses(List<Course> courses)
        {
            foreach (var course in courses)
            {
                await _courseRepository.InsertAsync(course);
            }
        }

        private async Task<UserLearningProgress> GetCurrentUserLearningProgress()
        {
            var learningProgress = await _userLearningProgressRepository.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId.Value);
            if (learningProgress is null)
            {
                var firstLessonId = await _lessonRepository.GetAll().Select(x => x.Id).FirstOrDefaultAsync();
                learningProgress = new UserLearningProgress
                {
                    UserId = AbpSession.UserId.Value,
                    Step = LessonStep.Score,
                    CurrentLessonId = firstLessonId,
                };
                await _userLearningProgressRepository.InsertAsync(learningProgress);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return learningProgress;
        }

        private async Task MoveToNewLesson(UserLearningProgress learningProgress)
        {
            // TODO: Implement moving to new lessons.
        }

        private async Task<LessonDto> GetLesson(int currentLessonId)
        {
            var query = _lessonRepository.GetAll().Where(x => x.Id == currentLessonId);
            var mappedQuery = ObjectMapper.ProjectTo<LessonDto>(query);
            var lesson = await mappedQuery.FirstOrDefaultAsync();
            return lesson;
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
                .Where(x => x.ProblemSetFk.LessonId == learningProgress.CurrentLessonId)
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
            _ => throw new NotImplementedException()
        };
    }
}
