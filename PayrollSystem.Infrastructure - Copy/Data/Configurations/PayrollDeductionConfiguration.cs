using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Payroll;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class PayrollDeductionConfiguration : IEntityTypeConfiguration<PayrollDeduction>
{
    public void Configure(EntityTypeBuilder<PayrollDeduction> builder)
    {
        builder.ToTable("PayrollDeductions");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Amount).HasColumnType("decimal(18,2)");
        builder.Property(d => d.Description).HasMaxLength(500);

        builder.HasOne(d => d.Employer)
            .WithMany()
            .HasForeignKey(d => d.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.PayrollTransaction)
            .WithMany(t => t.Deductions)
            .HasForeignKey(d => d.PayrollTransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.DeductionType)
            .WithMany()
            .HasForeignKey(d => d.DeductionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}