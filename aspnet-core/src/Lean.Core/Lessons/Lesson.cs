using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class Lesson : FullAuditedEntity
    {
        public string Name { get; set; }

        public bool IsInitial { get; set; }

        public string LessonText { get; set; }
        public string LessonVideoHtml { get; set; }

        public string ActivityText { get; set; }
        public string ActivityVideoHtml { get; set; }

        public int ModuleId { get; set; }
        public virtual Module ModuleFk { get; set; }

        public virtual List<Problem> Problems { get; set; }
    }
}
