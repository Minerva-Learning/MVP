using System;

namespace Lean.Url
{
    public class NullAppUrlService : IAppUrlService
    {
        public static IAppUrlService Instance { get; } = new NullAppUrlService();

        private NullAppUrlService()
        {
            
        }

        public string CreateEmailActivationUrlFormat(int? tenantId)
        {
            return "NotConfigured";
        }

        public string CreatePasswordResetUrlFormat(int? tenantId)
        {
            return "";
        }

        public string CreateEmailActivationUrlFormat(string tenancyName)
        {
             return "";
        }

        public string CreatePasswordResetUrlFormat(string tenancyName)
        {
             return "";
        }
    }
}
