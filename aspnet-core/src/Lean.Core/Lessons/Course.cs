using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class Course : FullAuditedEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Module> Modules { get; set; } = new List<Module>();
    }
}
