using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace Lean.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
