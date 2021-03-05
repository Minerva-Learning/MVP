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
    public class UserLessonAnswerSet : CreationAuditedEntity
    {
        public long UserId { get; set; }
        public User UserFk { get; set; }

        public int LessonId { get; set; }
        public Lesson LessonFk { get; set; }

        public virtual List<UserProblemAnswerResult> Answers { get; set; }
    }
}
