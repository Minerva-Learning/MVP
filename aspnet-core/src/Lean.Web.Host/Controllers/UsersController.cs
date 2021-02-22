using Abp.AspNetCore.Mvc.Authorization;
using Lean.Authorization;
using Lean.Storage;
using Abp.BackgroundJobs;

namespace Lean.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}