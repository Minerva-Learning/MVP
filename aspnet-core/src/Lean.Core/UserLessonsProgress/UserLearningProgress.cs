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
    public class UserLearningProgress : Entity
    {
        public LessonStep Step { get; set; }

        public int? CurrentProblemId { get; set; }
        public Problem CurrentProblemFk { get; set; }

        public int CurrentLessonId { get; set; }
        public Lesson CurrentLessonFk { get; set; }

        public long UserId { get; set; }
        public virtual User UserFk { get; set; }
    }
}
