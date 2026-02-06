using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Benefits;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployeeBenefitConfiguration : IEntityTypeConfiguration<EmployeeBenefit>
{
    public void Configure(EntityTypeBuilder<EmployeeBenefit> builder)
    {
        builder.ToTable("EmployeeBenefits");
        builder.HasKey(b => b.Id);

        builder.HasIndex(b => new { b.EmployerId, b.EmployeeId });

        builder.Property(b => b.EmployeeContribution).HasColumnType("decimal(18,2)");
        builder.Property(b => b.CoverageLevel).HasMaxLength(50);
        builder.Property(b => b.Status).HasMaxLength(50);

        builder.HasOne(b => b.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Employee)
            .WithMany(e => e.Benefits)
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.BenefitPlan)
            .WithMany(bp => bp.EmployeeBenefits)
            .HasForeignKey(b => b.BenefitPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}