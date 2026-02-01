using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Core.Entities.System;
using PayrollSystem.Core.Entities.Employers;
using PayrollSystem.Core.Entities.Employees;
using PayrollSystem.Core.Entities.Payroll;
using PayrollSystem.Core.Entities.Benefits;
using PayrollSystem.Core.Entities.Leaves;
using PayrollSystem.Core.Entities.TimeTracking;
using PayrollSystem.Core.Entities.Audit;
using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Interfaces;
using System.Linq.Expressions;

namespace PayrollSystem.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    private readonly ITenantService _tenantService;
    private readonly IEncryptionService _encryptionService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantService tenantService,
        IEncryptionService encryptionService)
        : base(options)
    {
        _tenantService = tenantService;
        _encryptionService = encryptionService;
    }

    // System-Wide Tables
    public DbSet<SystemSettings> SystemSettings { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<EmployerUser> EmployerUsers { get; set; }

    // Tenant-Specific Tables
    public DbSet<EmployerSettings> EmployerSettings { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeCompensation> EmployeeCompensations { get; set; }
    public DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
    public DbSet<TimeSheet> TimeSheets { get; set; }
    public DbSet<PayrollPeriod> PayrollPeriods { get; set; }
    public DbSet<PayrollTransaction> PayrollTransactions { get; set; }
    public DbSet<PayrollDeduction> PayrollDeductions { get; set; }
    public DbSet<PayrollTax> PayrollTaxes { get; set; }
    public DbSet<DeductionType> DeductionTypes { get; set; }
    public DbSet<EmployeeDeduction> EmployeeDeductions { get; set; }
    public DbSet<BenefitPlan> BenefitPlans { get; set; }
    public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<TaxBracket> TaxBrackets { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global query filters
        //ApplyTenantQueryFilters(modelBuilder);
        //ApplySoftDeleteFilters(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Customize Identity tables
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
        });
    }

    private void ApplyTenantQueryFilters(ModelBuilder modelBuilder)
    {
        // Skip query filters at design time (during migrations)
        if (_tenantService == null)
            return;

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                // For design time, don't apply the filter
                // The filter will work at runtime
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(GetTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
                    .MakeGenericMethod(entityType.ClrType);

                if (method != null)
                {
                    var filter = method.Invoke(null, new object[] { _tenantService });
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter((LambdaExpression)filter);
                }
            }
        }
    }

    private static LambdaExpression GetTenantFilter<TEntity>(ITenantService tenantService)
        where TEntity : TenantEntity
    {
        Expression<Func<TEntity, bool>> filter = e => e.EmployerId == tenantService.GetCurrentEmployerId();
        return filter;
    }
    private void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var filter = Expression.Lambda(Expression.Not(property), parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable && 
                   (e.State == EntityState.Added || e.State == EntityState.Modified));

        var currentTime = DateTime.UtcNow;
        var currentUserId = "system"; // TODO: Get from auth service

        foreach (var entry in entries)
        {
            var entity = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = currentTime;
                entity.CreatedBy = currentUserId;
            }

            entity.ModifiedAt = currentTime;
            entity.ModifiedBy = currentUserId;
        }

        // Set EmployerId for new TenantEntity objects
        var tenantEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is TenantEntity && e.State == EntityState.Added);

        foreach (var entry in tenantEntries)
        {
            var entity = (TenantEntity)entry.Entity;
            if (entity.EmployerId == 0)
            {
                try
                {
                    entity.EmployerId = _tenantService.GetCurrentEmployerId();
                }
                catch
                {
                    // During seeding, tenant context might not be set
                    // This is OK - seeder will set EmployerId explicitly
                }
            }
        }
    }

    public void SoftDelete<T>(T entity) where T : class, ISoftDelete
    {
        entity.IsDeleted = true;
        Entry(entity).State = EntityState.Modified;
    }
}