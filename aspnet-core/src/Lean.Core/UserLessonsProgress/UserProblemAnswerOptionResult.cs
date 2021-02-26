using Abp.Domain.Entities;
using Lean.Authorization.Users;
using Lean.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.UserLessonsProgress
{
    public class UserProblemAnswerOptionResult : Entity
    {
        public int ProblemAnswerOptionId { get; set; }
        public ProblemAnswerOption ProblemAnswerOptionFk { get; set; }

        public int UserProblemResultId { get; set; }
        public UserProblemResult UserProblemResultFk { get; set; }
    }
}
