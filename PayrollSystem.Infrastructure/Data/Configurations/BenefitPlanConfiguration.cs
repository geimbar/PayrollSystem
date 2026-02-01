using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Benefits;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class BenefitPlanConfiguration : IEntityTypeConfiguration<BenefitPlan>
{
    public void Configure(EntityTypeBuilder<BenefitPlan> builder)
    {
        builder.ToTable("BenefitPlans");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Description).HasMaxLength(1000);
        builder.Property(b => b.BenefitType).HasMaxLength(100);
        builder.Property(b => b.ProviderName).HasMaxLength(200);
        builder.Property(b => b.EmployerContribution).HasColumnType("decimal(18,2)");
        builder.Property(b => b.EmployeeContribution).HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Employer)
            .WithMany()
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}