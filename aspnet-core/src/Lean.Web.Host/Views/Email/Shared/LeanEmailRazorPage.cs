using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;
using Lean.Email.Model;
using Lean.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Web.Views.Email
{
    public abstract class LeanEmailRazorPage<TModel> : LeanRazorPage<TModel>
    {
        private readonly IWebUrlService _webUrlService;
        private readonly ITenantCache _tenantCache;

        protected LeanEmailRazorPage()
        {
            _webUrlService = IocManager.Resolve<IWebUrlService>();
            _tenantCache = IocManager.Resolve<ITenantCache>();
            Layout = "_EmailLayout";
        }

        protected IIocManager IocManager => Abp.Dependency.IocManager.Instance;

        protected string GetTenantLogoUrl(int? tenantId)
        {
            if (!tenantId.HasValue)
            {
                return _webUrlService.GetServerRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?skin=light";
            }

            var tenant = _tenantCache.Get(tenantId.Value);
            return _webUrlService.GetServerRootAddress(tenant.TenancyName).EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?skin=light&tenantId=" + tenantId.Value;
        }

        protected string TryGetTitle() =>
            Model is CommonEmailModel model ? model.Title : string.Empty;
    }

    public abstract class LeanEmailLayoutRazorPage<TModel> : LeanEmailRazorPage<TModel>
    {
        protected LeanEmailLayoutRazorPage()
            : base()
        {
            Layout = null;
        }
    }
}
