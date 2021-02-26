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
        public string Name { get; set; }

        public string TaskDescription { get; set; }

        public ProblemType Type { get; set; }

        public int ProblemSetId { get; set; }

        public virtual ProblemSet ProblemSetFk { get; set; }

        public virtual List<ProblemAnswerOption> ProblemAnswerOptions { get; set; }
    }
}
