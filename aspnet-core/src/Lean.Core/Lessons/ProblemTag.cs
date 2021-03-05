using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons
{
    public class ProblemTag
    {
        public int ProblemId { get; set; }
        public Problem ProblemFk { get; set; }

        public int TagId { get; set; }
        public Tag TagFk { get; set; }

        public decimal ProblemTagRating { get; set; }
    }
}
