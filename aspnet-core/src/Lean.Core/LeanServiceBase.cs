using Abp;
using Abp.Extensions;
using Abp.Timing;
using Abp.Timing.Timezone;
using Lean.Timing;
using System;
using System.Threading.Tasks;

namespace Lean
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="LeanDomainServiceBase"/>.
    /// For application services inherit LeanAppServiceBase.
    /// </summary>
    public abstract class LeanServiceBase : AbpServiceBase
    {
        protected LeanServiceBase()
        {
            LocalizationSourceName = LeanConsts.LocalizationSourceName;
        }

        public ITimeZoneService TimeZoneService { get; set; }

        protected virtual Task<TimeZoneInfo> GetUserTimezone(UserIdentifier userIdentifier)
        {
            return GetUserTimezone(userIdentifier.UserId, userIdentifier.TenantId);
        }

        protected virtual async Task<TimeZoneInfo> GetUserTimezone(long userId, int? tenantId)
        {
            var timezoneId = await SettingManager.GetSettingValueForUserAsync(
                TimingSettingNames.TimeZone,
                tenantId,
                userId);
            if (timezoneId.IsNullOrEmpty())
            {
                timezoneId = await TimeZoneService.GetDefaultTimezoneAsync(Abp.Configuration.SettingScopes.User, tenantId);
            }

            var timezone = TimezoneHelper.FindTimeZoneInfo(timezoneId);
            return timezone;
        }
    }
}