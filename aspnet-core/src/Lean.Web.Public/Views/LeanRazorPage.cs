using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Lean.Web.Public.Views
{
    public abstract class LeanRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected LeanRazorPage()
        {
            LocalizationSourceName = LeanConsts.LocalizationSourceName;
        }
    }
}
