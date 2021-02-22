using System.Threading.Tasks;
using Abp.Application.Services;
using Lean.Configuration.Tenants.Dto;

namespace Lean.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
