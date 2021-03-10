using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class FlowRuleProblem : Entity
    {
        public int FlowRuleId { get; set; }
        public int ProblemId { get; set; }
        public virtual Problem ProblemFk { get; set; }
    }
}
