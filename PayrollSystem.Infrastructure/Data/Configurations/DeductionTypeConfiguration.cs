using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Payroll;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class DeductionTypeConfiguration : IEntityTypeConfiguration<DeductionType>
{
    public void Configure(EntityTypeBuilder<DeductionType> builder)
    {
        builder.ToTable("DeductionTypes");
        builder.HasKey(d => d.Id);

        builder.HasIndex(d => new { d.EmployerId, d.Name });

        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.Property(d => d.DeductionCategory).HasMaxLength(50);
        builder.Property(d => d.DefaultAmount).HasColumnType("decimal(18,2)");

        builder.HasOne(d => d.Employer)
            .WithMany()
            .HasForeignKey(d => d.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}