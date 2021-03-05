using Lean.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Web
{
    public static class ConfigureOptions
    {
        public static void AddAppOptions(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddOptions<RatingVariables>().Bind(configurationRoot.GetSection("App:RatingVariables"));
        }
    }
}
