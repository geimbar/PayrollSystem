using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployerUserConfiguration : IEntityTypeConfiguration<EmployerUser>
{
    public void Configure(EntityTypeBuilder<EmployerUser> builder)
    {
        builder.ToTable("EmployerUsers");
        builder.HasKey(eu => eu.Id);
        
        builder.Property(eu => eu.Role).IsRequired().HasMaxLength(50);
        
        builder.HasIndex(eu => new { eu.UserId, eu.EmployerId }).IsUnique();
        
        builder.HasOne(eu => eu.User)
            .WithMany(u => u.EmployerUsers)
            .HasForeignKey(eu => eu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(eu => eu.Employer)
            .WithMany(e => e.EmployerUsers)
            .HasForeignKey(eu => eu.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}