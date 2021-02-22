using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Lean.Authorization.Users;
using Lean.MultiTenancy;

namespace Lean.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}