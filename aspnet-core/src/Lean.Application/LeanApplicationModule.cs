using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Lean.Authorization;
using Lean.Email;

namespace Lean
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(LeanApplicationSharedModule),
        typeof(LeanCoreModule)
        )]
    public class LeanApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanApplicationModule).GetAssembly());
            //IocManager.Register<IViewRenderService, ViewRenderService>();
            //IocManager.Register<ISiteRootProvider, SiteRootProvider>();
            //IocManager.Register<IEmailBodyRenderer, EmailBodyRenderer>();
        }
    }
}