using System.Threading.Tasks;
using Lean.Authorization.Users;

namespace Lean.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
