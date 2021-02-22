using Abp.Domain.Services;

namespace Lean
{
    public abstract class LeanDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected LeanDomainServiceBase()
        {
            LocalizationSourceName = LeanConsts.LocalizationSourceName;
        }
    }
}
