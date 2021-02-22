using System.Threading.Tasks;

namespace Lean.Email
{
    public interface IEmailService
    {
        Task SendTestEmail();
    }
}