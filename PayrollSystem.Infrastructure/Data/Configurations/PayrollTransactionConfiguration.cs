using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Payroll;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class PayrollTransactionConfiguration : IEntityTypeConfiguration<PayrollTransaction>
{
    public void Configure(EntityTypeBuilder<PayrollTransaction> builder)
    {
        builder.ToTable("PayrollTransactions");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => new { t.EmployerId, t.PayrollPeriodId });
        builder.HasIndex(t => new { t.EmployerId, t.EmployeeId });
        builder.HasIndex(t => new { t.EmployerId, t.PaymentStatus });

        // Decimal properties with proper precision
        builder.Property(t => t.GrossPay).HasColumnType("decimal(18,2)");
        builder.Property(t => t.NetPay).HasColumnType("decimal(18,2)");
        builder.Property(t => t.RegularHours).HasColumnType("decimal(8,2)");
        builder.Property(t => t.OvertimeHours).HasColumnType("decimal(8,2)");
        builder.Property(t => t.RegularPay).HasColumnType("decimal(18,2)");
        builder.Property(t => t.OvertimePay).HasColumnType("decimal(18,2)");
        builder.Property(t => t.BonusPay).HasColumnType("decimal(18,2)");
        builder.Property(t => t.CommissionPay).HasColumnType("decimal(18,2)");
        builder.Property(t => t.TotalDeductions).HasColumnType("decimal(18,2)");
        builder.Property(t => t.TotalTaxes).HasColumnType("decimal(18,2)");

        builder.Property(t => t.PaymentMethod).HasMaxLength(50);
        builder.Property(t => t.PaymentStatus).HasMaxLength(50);
        builder.Property(t => t.CheckNumber).HasMaxLength(50);

        builder.HasOne(t => t.Employer)
            .WithMany()
            .HasForeignKey(t => t.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Employee)
            .WithMany(e => e.PayrollTransactions)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.PayrollPeriod)
            .WithMany(p => p.PayrollTransactions)
            .HasForeignKey(t => t.PayrollPeriodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}