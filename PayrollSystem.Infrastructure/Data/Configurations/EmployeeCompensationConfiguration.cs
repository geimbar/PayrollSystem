using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployeeCompensationConfiguration : IEntityTypeConfiguration<EmployeeCompensation>
{
    public void Configure(EntityTypeBuilder<EmployeeCompensation> builder)
    {
        builder.ToTable("EmployeeCompensations");
        builder.HasKey(c => c.Id);
        
        builder.HasIndex(c => new { c.EmployerId, c.EmployeeId });
        builder.HasIndex(c => new { c.EmployerId, c.EffectiveDate });
        
        builder.Property(c => c.CompensationType).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Amount).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Currency).HasMaxLength(10);
        
        builder.HasOne(c => c.Employer)
            .WithMany()
            .HasForeignKey(c => c.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.Employee)
            .WithMany(e => e.Compensations)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}