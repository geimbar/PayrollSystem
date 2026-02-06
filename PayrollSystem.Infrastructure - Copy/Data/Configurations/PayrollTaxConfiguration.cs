using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Payroll;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class PayrollTaxConfiguration : IEntityTypeConfiguration<PayrollTax>
{
    public void Configure(EntityTypeBuilder<PayrollTax> builder)
    {
        builder.ToTable("PayrollTaxes");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TaxType).IsRequired().HasMaxLength(50);
        builder.Property(t => t.TaxableAmount).HasColumnType("decimal(18,2)");
        builder.Property(t => t.TaxRate).HasColumnType("decimal(5,4)");
        builder.Property(t => t.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(t => t.Description).HasMaxLength(500);

        builder.HasOne(t => t.Employer)
            .WithMany()
            .HasForeignKey(t => t.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.PayrollTransaction)
            .WithMany(pt => pt.Taxes)
            .HasForeignKey(t => t.PayrollTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}