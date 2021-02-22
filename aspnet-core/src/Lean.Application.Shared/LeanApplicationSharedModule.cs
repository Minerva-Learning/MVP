using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lean
{
    [DependsOn(typeof(LeanCoreSharedModule))]
    public class LeanApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanApplicationSharedModule).GetAssembly());
        }
    }
}