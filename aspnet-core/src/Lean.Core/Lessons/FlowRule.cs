using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class FlowRule : Entity
    {
        public int LessonId { get; set; }
        public virtual Lesson LessonFk { get; set; }

        public FlowCondition Condition { get; set; }

        public int? CorrectAnswersCount { get; set; }

        public virtual List<FlowRuleProblem> FlowRuleProblems { get; set; } = new List<FlowRuleProblem>();

        public virtual List<FlowRuleNextLesson> FlowRuleNextLessons { get; set; } = new List<FlowRuleNextLesson>();
    }
}
