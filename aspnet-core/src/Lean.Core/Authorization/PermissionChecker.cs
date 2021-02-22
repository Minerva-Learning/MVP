using Abp.Authorization;
using Lean.Authorization.Roles;
using Lean.Authorization.Users;

namespace Lean.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
