using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.ToTable("Employers");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
        builder.Property(e => e.TaxIdentificationNumber).HasMaxLength(50);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(256);
        builder.Property(e => e.SubscriptionPlan).HasMaxLength(50);
        builder.Property(e => e.SubscriptionStatus).HasMaxLength(50);
        builder.Property(e => e.RowVersion).IsRowVersion();
        
        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.TaxIdentificationNumber);
        
        builder.HasOne(e => e.Settings)
            .WithOne()
            .HasForeignKey<EmployerSettings>(s => s.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}