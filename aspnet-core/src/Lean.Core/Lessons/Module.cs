using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class Module : FullAuditedEntity
    {
        public string Name { get; set; }

        public int Priority { get; set; }

        public int CourseId { get; set; }

        public virtual Course CourseFk { get; set; }

        public virtual List<Lesson> Lessons { get; set; } = new List<Lesson>();

        public virtual List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
