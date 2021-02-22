using System.Threading.Tasks;

namespace Lean.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}