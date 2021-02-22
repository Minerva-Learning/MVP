using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lean
{
    [DependsOn(typeof(LeanXamarinSharedModule))]
    public class LeanXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanXamarinAndroidModule).GetAssembly());
        }
    }
}