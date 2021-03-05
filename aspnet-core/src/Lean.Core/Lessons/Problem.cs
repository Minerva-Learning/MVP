using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class Problem : FullAuditedEntity
    {
        public int Number { get; set; }

        public string TaskDescription { get; set; }

        public ProblemType Type { get; set; }

        public int LessonId { get; set; }
        public virtual Lesson LessonFk { get; set; }

        public virtual List<ProblemAnswerOption> ProblemAnswerOptions { get; set; }

        public virtual List<ProblemTag> ProblemTags { get; set; }
    }
}
