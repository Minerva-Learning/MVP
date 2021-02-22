using System.Threading.Tasks;
using Abp.Application.Services;
using Lean.MultiTenancy.Payments.PayPal.Dto;

namespace Lean.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
