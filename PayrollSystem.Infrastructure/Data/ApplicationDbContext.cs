using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Core.Entities.Core;
using PayrollSystem.Core.Entities.Identity;
using PayrollSystem.Core.Entities.People;
using PayrollSystem.Core.Entities.Employment;
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

    // Core DbSets
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<UserTenant> UserTenants { get; set; }

    // People DbSets
    public DbSet<Employee> Employees { get; set; }
    public DbSet<NextOfKin> NextOfKins { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }

    // Employment DbSets
    public DbSet<Employment> Employments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Compensation> Compensations { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }

    // Tax DbSets
    public DbSet<TaxCard> TaxCards { get; set; }

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

        // Configure cascade delete behavior to avoid cycles

        // Tenant -> Companies (Cascade)
        modelBuilder.Entity<Company>()
            .HasOne(c => c.Tenant)
            .WithMany(t => t.Companies)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Tenant -> Employees (Cascade)
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Tenant)
            .WithMany(t => t.Employees)
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Company -> Departments (Cascade)
        modelBuilder.Entity<Department>()
            .HasOne(d => d.Company)
            .WithMany(c => c.Departments)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employee -> Employments (NO ACTION - avoid cycle through Company)
        modelBuilder.Entity<Employment>()
            .HasOne(e => e.Employee)
            .WithMany(emp => emp.Employments)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        // Company -> Employments (NO ACTION - avoid cycle through Employee)
        modelBuilder.Entity<Employment>()
            .HasOne(e => e.Company)
            .WithMany(c => c.Employments)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        // Department -> Employments (NO ACTION)
        modelBuilder.Entity<Employment>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employments)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        // Employment -> Compensations (Cascade)
        modelBuilder.Entity<Compensation>()
            .HasOne(c => c.Employment)
            .WithMany(e => e.Compensations)
            .HasForeignKey(c => c.EmploymentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employment -> LeaveBalances (Cascade)
        modelBuilder.Entity<LeaveBalance>()
            .HasOne(lb => lb.Employment)
            .WithMany(e => e.LeaveBalances)
            .HasForeignKey(lb => lb.EmploymentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employee -> TaxCards (Cascade)
        modelBuilder.Entity<TaxCard>()
            .HasOne(tc => tc.Employee)
            .WithMany(e => e.TaxCards)
            .HasForeignKey(tc => tc.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employee -> NextOfKins (Cascade)
        modelBuilder.Entity<NextOfKin>()
            .HasOne(nok => nok.Employee)
            .WithMany(e => e.NextOfKins)
            .HasForeignKey(nok => nok.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employee -> BankAccounts (Cascade)
        modelBuilder.Entity<BankAccount>()
            .HasOne(ba => ba.Employee)
            .WithMany(e => e.BankAccounts)
            .HasForeignKey(ba => ba.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure decimal precision for money/numeric fields
        modelBuilder.Entity<Employment>()
            .Property(e => e.BaseSalary)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Employment>()
            .Property(e => e.HoursPerWeek)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Compensation>()
            .Property(c => c.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<LeaveBalance>()
            .Property(lb => lb.TotalDays)
            .HasPrecision(5, 2);

        modelBuilder.Entity<LeaveBalance>()
            .Property(lb => lb.UsedDays)
            .HasPrecision(5, 2);

        modelBuilder.Entity<LeaveBalance>()
            .Property(lb => lb.RemainingDays)
            .HasPrecision(5, 2);

        modelBuilder.Entity<TaxCard>()
            .Property(tc => tc.TaxRate)
            .HasPrecision(5, 4);

        modelBuilder.Entity<TaxCard>()
            .Property(tc => tc.DeductionAmount)
            .HasPrecision(18, 2);

        // Additional configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
