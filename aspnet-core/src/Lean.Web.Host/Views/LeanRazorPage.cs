using Abp.AspNetCore.Mvc.Views;

namespace Lean.Web.Views
{
    public abstract class LeanRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected LeanRazorPage()
        {
            LocalizationSourceName = LeanConsts.LocalizationSourceName;
        }
    }
}
