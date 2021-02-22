using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Lean.Authorization.Permissions.Dto;

namespace Lean.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
