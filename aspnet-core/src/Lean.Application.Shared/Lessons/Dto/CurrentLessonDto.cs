using Lean.UserLessonsProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons.Dto
{
    public class CurrentLessonDto
    {
        public LessonDto Lesson { get; set; }

        public LessonStep Step { get; set; }

        public ProblemDto Problem { get; set; }

        public List<TagScoreDto> TagScores {get;set;}
    }
}
