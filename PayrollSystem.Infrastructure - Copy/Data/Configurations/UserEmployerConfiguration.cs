using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class UserEmployerConfiguration : IEntityTypeConfiguration<UserEmployer>
{
    public void Configure(EntityTypeBuilder<UserEmployer> builder)
    {
        builder.HasKey(ue => new { ue.UserId, ue.EmployerId });

        builder.HasOne(ue => ue.User)
            .WithMany(u => u.UserEmployers)
            .HasForeignKey(ue => ue.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ue => ue.Employer)
            .WithMany()
            .HasForeignKey(ue => ue.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ue => ue.Role)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(ue => ue.EmployerId);
    }
}
