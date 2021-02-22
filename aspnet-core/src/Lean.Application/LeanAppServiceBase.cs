using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Identity;
using Lean.Authorization.Users;
using Lean.MultiTenancy;
using Abp.Timing;
using Lean.Timing;
using Abp.Extensions;
using Abp.Timing.Timezone;

namespace Lean
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class LeanAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public ITimeZoneService TimeZoneService { get; set; }

        protected LeanAppServiceBase()
        {
            LocalizationSourceName = LeanConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }

        protected virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual async Task<TimeZoneInfo> GetCurrentUserTimezone()
        {
            var timezoneId = await SettingManager.GetSettingValueForUserAsync(
                TimingSettingNames.TimeZone,
                AbpSession.ToUserIdentifier());
            if (timezoneId.IsNullOrEmpty())
            {
                timezoneId = await TimeZoneService.GetDefaultTimezoneAsync(Abp.Configuration.SettingScopes.User, AbpSession.TenantId);
            }

            var timezone = TimezoneHelper.FindTimeZoneInfo(timezoneId);
            return timezone;
        }
    }
}