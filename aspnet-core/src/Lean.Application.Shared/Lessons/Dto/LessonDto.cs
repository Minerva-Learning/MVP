using Abp.Application.Services.Dto;
using Lean.UserLessonsProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons.Dto
{
    public class LessonDto : EntityDto
    {
        public string Name { get; set; }

        public string LessonText { get; set; }

        public string LessonVideoUrl { get; set; }

        public string ActivityText { get; set; }

        public string ActivityVideoUrl { get; set; }

        public LessonStep Step { get; set; }
    }
}
