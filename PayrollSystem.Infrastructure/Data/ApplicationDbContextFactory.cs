using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PayrollSystem.Core.Interfaces;

namespace PayrollSystem.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "../PayrollSystem.Web/appsettings.Development.json"))
            .Build();

        // Create options
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        // Create mock services for design time
        var tenantService = new DesignTimeTenantService();
        var encryptionService = new DesignTimeEncryptionService(configuration);

        return new ApplicationDbContext(optionsBuilder.Options, tenantService, encryptionService);
    }

    // Mock tenant service for design time
    private class DesignTimeTenantService : ITenantService
    {
        public int GetCurrentEmployerId() => 0; // Not used at design time
        public void SetCurrentEmployer(int employerId) { }
        public bool HasAccess(int employerId) => true;
    }

    // Mock encryption service for design time
    private class DesignTimeEncryptionService : IEncryptionService
    {
        public DesignTimeEncryptionService(IConfiguration configuration) { }
        public string Encrypt(string plainText) => plainText;
        public string Decrypt(string cipherText) => cipherText;
    }
}