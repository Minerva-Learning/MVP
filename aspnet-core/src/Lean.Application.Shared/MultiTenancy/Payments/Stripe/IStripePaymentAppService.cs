using System.Threading.Tasks;
using Abp.Application.Services;
using Lean.MultiTenancy.Payments.Dto;
using Lean.MultiTenancy.Payments.Stripe.Dto;

namespace Lean.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}