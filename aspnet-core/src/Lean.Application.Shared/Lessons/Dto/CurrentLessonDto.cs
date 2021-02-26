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

        public ProblemDto Problem { get; set; }
    }
}
