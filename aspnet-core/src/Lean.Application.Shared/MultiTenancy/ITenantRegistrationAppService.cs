using System.Threading.Tasks;
using Abp.Application.Services;
using Lean.Editions.Dto;
using Lean.MultiTenancy.Dto;

namespace Lean.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}