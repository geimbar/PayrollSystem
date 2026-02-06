using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Core.Entities;
using PayrollSystem.Core.Entities.Core;
using PayrollSystem.Core.Entities.Employment;
using PayrollSystem.Core.Entities.Identity;
using PayrollSystem.Core.Entities.People;
using PayrollSystem.Core.Entities.Tax;
using PayrollSystem.Infrastructure.Services;

namespace PayrollSystem.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantService _tenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    // DbSets
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Employment> Employments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<TaxCard> TaxCards { get; set; }
    public DbSet<NextOfKin> NextOfKins { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Compensation> Compensations { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }
    public DbSet<UserTenant> UserTenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Query Filters - Filter by TenantId
        var tenantId = _tenantService.GetCurrentTenantId();

        modelBuilder.Entity<Tenant>()
            .HasQueryFilter(t => !tenantId.HasValue || t.Id == tenantId.Value);

        modelBuilder.Entity<Company>()
            .HasQueryFilter(c => !tenantId.HasValue || c.TenantId == tenantId.Value);

        modelBuilder.Entity<Employee>()
            .HasQueryFilter(e => !tenantId.HasValue || e.TenantId == tenantId.Value);

        modelBuilder.Entity<Employment>()
            .HasQueryFilter(e => !tenantId.HasValue || e.Employee.TenantId == tenantId.Value);

        modelBuilder.Entity<Department>()
            .HasQueryFilter(d => !tenantId.HasValue || d.Company.TenantId == tenantId.Value);

        // UserTenant composite key
        modelBuilder.Entity<UserTenant>()
            .HasKey(ut => new { ut.UserId, ut.TenantId });

        // ApplicationUser relationships
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.PrimaryTenant)
            .WithMany()
            .HasForeignKey(u => u.PrimaryTenantId)
            .OnDelete(DeleteBehavior.SetNull);

        // Additional configurations can be in separate files
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}