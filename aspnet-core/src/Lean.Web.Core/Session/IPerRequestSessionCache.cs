using System.Threading.Tasks;
using Lean.Sessions.Dto;

namespace Lean.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
