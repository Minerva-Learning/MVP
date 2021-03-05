using Abp.Domain.Entities;
using Lean.Authorization.Users;
using Lean.Lessons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.UserLessonsProgress
{
    public class UserTagRating : Entity<string>
    {
        [NotMapped]
        public override string Id 
        { 
            get => $"{UserId}:{TagId}";
            set {}
        }

        public long UserId { get; set; }
        public User UserFk { get; set; }

        public int TagId { get; set; }
        public Tag TagFk { get; set; }

        public decimal Rating { get; set; }
    }
}
