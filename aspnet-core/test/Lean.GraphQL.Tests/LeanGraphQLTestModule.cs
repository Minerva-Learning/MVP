using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Lean.Configure;
using Lean.Startup;
using Lean.Test.Base;

namespace Lean.GraphQL.Tests
{
    [DependsOn(
        typeof(LeanGraphQLModule),
        typeof(LeanTestBaseModule))]
    public class LeanGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanGraphQLTestModule).GetAssembly());
        }
    }
}