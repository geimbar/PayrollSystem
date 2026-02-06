using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PayrollSystem.Infrastructure.Services;

namespace PayrollSystem.Infrastructure.Data;

/// <summary>
/// Factory for creating ApplicationDbContext at design time (for migrations)
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        // Use a design-time tenant service
        var tenantService = new DesignTimeTenantService();

        return new ApplicationDbContext(optionsBuilder.Options, tenantService);
    }

    /// <summary>
    /// Design-time implementation of ITenantService
    /// Returns null for all values since we don't have tenant context at design time
    /// </summary>
    private class DesignTimeTenantService : ITenantService
    {
        public int? GetCurrentTenantId() => null;

        public int? GetCurrentCompanyId() => null;

        public void SetCurrentTenant(int tenantId, int? companyId = null)
        {
            // No-op at design time
        }
    }
}