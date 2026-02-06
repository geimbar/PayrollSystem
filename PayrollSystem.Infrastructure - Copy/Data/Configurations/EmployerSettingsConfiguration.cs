using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployerSettingsConfiguration : IEntityTypeConfiguration<EmployerSettings>
{
    public void Configure(EntityTypeBuilder<EmployerSettings> builder)
    {
        builder.ToTable("EmployerSettings");
        builder.HasKey(s => s.Id);
        
        builder.HasIndex(s => new { s.EmployerId, s.Id });
        
        builder.Property(s => s.DefaultPayPeriodType).HasMaxLength(50);
        builder.Property(s => s.DefaultCurrency).HasMaxLength(10);
        builder.Property(s => s.TimeZone).HasMaxLength(100);
        builder.Property(s => s.OvertimeMultiplier).HasColumnType("decimal(5,2)").HasDefaultValue(1.5m);
        
        builder.HasOne(s => s.Employer)
            .WithOne(e => e.Settings)
            .HasForeignKey<EmployerSettings>(s => s.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}