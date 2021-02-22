using System.Threading.Tasks;
using Abp.Application.Services;

namespace Lean.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
