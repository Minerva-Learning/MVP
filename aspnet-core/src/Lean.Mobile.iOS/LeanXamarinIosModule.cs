using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lean
{
    [DependsOn(typeof(LeanXamarinSharedModule))]
    public class LeanXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanXamarinIosModule).GetAssembly());
        }
    }
}