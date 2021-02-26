using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons.Dto
{
    public class ProblemDto : EntityDto
    {
        public string Name { get; set; }

        public string TaskDescription { get; set; }

        public ProblemType Type { get; set; }

        public virtual List<ProblemAnswerOptionDto> ProblemAnswerOptions { get; set; }
    }
}
