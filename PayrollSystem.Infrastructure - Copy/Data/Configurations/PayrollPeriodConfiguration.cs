using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Payroll;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class PayrollPeriodConfiguration : IEntityTypeConfiguration<PayrollPeriod>
{
    public void Configure(EntityTypeBuilder<PayrollPeriod> builder)
    {
        builder.ToTable("PayrollPeriods");
        builder.HasKey(p => p.Id);
        
        builder.HasIndex(p => new { p.EmployerId, p.StartDate, p.EndDate });
        builder.HasIndex(p => new { p.EmployerId, p.Status });
        
        builder.Property(p => p.PeriodType).HasMaxLength(50);
        builder.Property(p => p.Status).HasMaxLength(50);
        
        builder.HasOne(p => p.Employer)
            .WithMany()
            .HasForeignKey(p => p.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}