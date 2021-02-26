using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class ProblemSet : FullAuditedEntity
    {
        public int LessonId { get; set; }

        public virtual Lesson LessonFk { get; set; }

        public virtual List<Problem> Problems { get; set; }
    }
}
