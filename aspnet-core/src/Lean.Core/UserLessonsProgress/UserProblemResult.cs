using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Lean.Authorization.Users;
using Lean.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.UserLessonsProgress
{
    public class UserProblemResult : CreationAuditedEntity
    {
        public string TextAnswer { get; set; }

        public bool IsCorrect { get; set; }

        public int ProblemId { get; set; }
        public Problem ProblemFk { get; set; }

        public long UserId { get; set; }
        public User UserFk { get; set; }
    }
}
