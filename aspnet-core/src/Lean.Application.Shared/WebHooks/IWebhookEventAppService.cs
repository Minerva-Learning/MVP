using System.Threading.Tasks;
using Abp.Webhooks;

namespace Lean.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
