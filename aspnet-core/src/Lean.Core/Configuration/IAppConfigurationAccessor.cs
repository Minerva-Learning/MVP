using Microsoft.Extensions.Configuration;

namespace Lean.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
