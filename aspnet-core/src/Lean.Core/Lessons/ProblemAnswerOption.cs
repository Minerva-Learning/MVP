using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class ProblemAnswerOption : Entity
    {
        public bool IsCorrect { get; set; }

        public string Text { get; set; }

        public int ProblemId { get; set; }

        public virtual Problem ProblemFk { get; set; }
    }
}
