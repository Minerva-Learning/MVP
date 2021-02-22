using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lean
{
    public class LeanCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanCoreSharedModule).GetAssembly());
        }
    }
}