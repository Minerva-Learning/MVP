using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class FlowRuleNextLesson : Entity
    {
        public int FlowRuleId { get; set; }

        public int NextLessonId { get; set; }
        public virtual Lesson NextLessonFk { get; set; }

        public int Priority { get; set; }
    }
}
