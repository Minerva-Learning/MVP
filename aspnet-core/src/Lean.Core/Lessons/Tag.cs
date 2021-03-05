using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class Tag : Entity
    {
        public string Name { get; set; }

        public int InitialRating { get; set; }

        public int ModuleId { get; set; }
        public virtual Module ModuleFk { get; set; }

        public virtual List<ProblemTag> ProblemTags { get; set; }
    }
}
