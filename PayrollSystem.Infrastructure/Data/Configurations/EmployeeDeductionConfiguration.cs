using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Payroll;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployeeDeductionConfiguration : IEntityTypeConfiguration<EmployeeDeduction>
{
    public void Configure(EntityTypeBuilder<EmployeeDeduction> builder)
    {
        builder.ToTable("EmployeeDeductions");
        builder.HasKey(d => d.Id);

        builder.HasIndex(d => new { d.EmployerId, d.EmployeeId });

        builder.Property(d => d.Amount).HasColumnType("decimal(18,2)");
        builder.Property(d => d.Percentage).HasColumnType("decimal(5,2)");

        builder.HasOne(d => d.Employer)
            .WithMany()
            .HasForeignKey(d => d.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Employee)
            .WithMany(e => e.Deductions)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.DeductionType)
            .WithMany(dt => dt.EmployeeDeductions)
            .HasForeignKey(d => d.DeductionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}