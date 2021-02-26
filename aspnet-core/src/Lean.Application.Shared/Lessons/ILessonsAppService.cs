using Lean.Lessons.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public interface ILessonsAppService
    {
        Task<CurrentLessonDto> GetCurrentLesson();

        Task<CurrentLessonDto> MoveToNextStep();
    }
}
