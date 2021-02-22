using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lean
{
    [DependsOn(typeof(LeanClientModule), typeof(AbpAutoMapperModule))]
    public class LeanXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanXamarinSharedModule).GetAssembly());
        }
    }
}