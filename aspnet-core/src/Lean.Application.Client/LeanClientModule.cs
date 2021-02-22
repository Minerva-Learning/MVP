using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lean
{
    public class LeanClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LeanClientModule).GetAssembly());
        }
    }
}
