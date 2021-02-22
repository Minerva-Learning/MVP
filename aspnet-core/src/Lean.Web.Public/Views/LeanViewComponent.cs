using Abp.AspNetCore.Mvc.ViewComponents;

namespace Lean.Web.Public.Views
{
    public abstract class LeanViewComponent : AbpViewComponent
    {
        protected LeanViewComponent()
        {
            LocalizationSourceName = LeanConsts.LocalizationSourceName;
        }
    }
}