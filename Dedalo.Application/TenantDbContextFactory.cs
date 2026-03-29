using Dedalo.Domain.Interfaces;
using Dedalo.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Dedalo.Application
{
    public class TenantDbContextFactory
    {
        private readonly ITenantResolver _tenantResolver;

        public TenantDbContextFactory(ITenantResolver tenantResolver)
        {
            _tenantResolver = tenantResolver;
        }

        public DedaloContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DedaloContext>();
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_tenantResolver.ConnectionString);
            return new DedaloContext(optionsBuilder.Options);
        }
    }
}
