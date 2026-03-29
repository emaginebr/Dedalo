using NAuth.ACL.Interfaces;
using Dedalo.Domain.Interfaces;

namespace Dedalo.Application
{
    public class NAuthTenantProvider : ITenantProvider
    {
        private readonly ITenantContext _tenantContext;

        public NAuthTenantProvider(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public string? GetTenantId()
        {
            try
            {
                return _tenantContext.TenantId;
            }
            catch
            {
                return null;
            }
        }
    }
}
