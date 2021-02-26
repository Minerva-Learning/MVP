using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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

        public LessonsAppService(
            IRepository<Course> courseRepository,
            IRepository<Module> moduleRepository,
            IRepository<Lesson> lessonRepository,
            IRepository<Problem> problemRepository,
            IRepository<UserLearningProgress> userLearningProgressRepository
            )
        {
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
            _lessonRepository = lessonRepository;
            _problemRepository = problemRepository;
            _userLearningProgressRepository = userLearningProgressRepository;
        }

        public async Task<CurrentLessonDto> GetCurrentLesson()
        {
            var learningProgress = await GetCurrentUserLearningProgress();
            var currentLesson = new CurrentLessonDto();
            LessonDto lesson = await GetLesson(learningProgress.CurrentLessonId);
            lesson.Step = learningProgress.Step;

            if (learningProgress.Step == LessonStep.ProblemSet)
            {
                var problemQuery = _problemRepository.GetAll().Where(x => x.Id == learningProgress.CurrentProblemId);
                var problem = await ObjectMapper.ProjectTo<ProblemDto>(problemQuery).FirstOrDefaultAsync();
                currentLesson.Problem = problem;
            }

            return currentLesson;
        }

        public async Task<CurrentLessonDto> MoveToNextStep()
        {
            var learningProgress = await _userLearningProgressRepository.GetAll().AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId.Value);
            var nextStep = GetNextStep(learningProgress.Step);

            if (nextStep == LessonStep.Lesson)
            {
                // Moving to the next lesson logic.
            }

            var currentLesson = new CurrentLessonDto
            {
                Lesson = await GetLesson(learningProgress.CurrentLessonId),
                Problem = await GetProblem(learningProgress)
            };

            learningProgress.Step = nextStep;
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
            var learningProgress = await _userLearningProgressRepository.GetAll().AsNoTracking()
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
                var problemQuery = _problemRepository.GetAll().Where(x => x.Id == learningProgress.CurrentProblemId);
                var problem = await ObjectMapper.ProjectTo<ProblemDto>(problemQuery).FirstOrDefaultAsync();
                return problem;
            }

            return null;
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
