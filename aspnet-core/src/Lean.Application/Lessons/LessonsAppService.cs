using Abp.Authorization;
using Abp.Domain.Repositories;
using Lean.Lessons.Dto;
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
        private readonly IRepository<ProblemSet> _problemSetRepository;

        public LessonsAppService(
            IRepository<Course> courseRepository,
            IRepository<Module> moduleRepository,
            IRepository<Lesson> lessonRepository,
            IRepository<ProblemSet> problemSetRepository
            )
        {
            _courseRepository = courseRepository;
            _moduleRepository = moduleRepository;
            _lessonRepository = lessonRepository;
            _problemSetRepository = problemSetRepository;
        }

        public async Task<LessonDto> GetCurrentLesson()
        {
            var query = _lessonRepository.GetAll();
            var mappedQuery = ObjectMapper.ProjectTo<LessonDto>(query);
            return await mappedQuery.FirstOrDefaultAsync();
        }
    }
}
