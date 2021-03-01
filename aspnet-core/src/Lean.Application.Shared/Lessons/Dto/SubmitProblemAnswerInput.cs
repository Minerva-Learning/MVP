using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons.Dto
{
    public class SubmitProblemAnswerInput
    {
        [Required]
        public int ProblemId { get; set; }

        public string FreeTextAnswer { get; set; }

        public int? ChoiseId { get; set; }
    }
}
