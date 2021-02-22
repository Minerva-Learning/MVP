using System.Threading.Tasks;
using Abp.Application.Services;
using Lean.Sessions.Dto;

namespace Lean.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
