using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(d => d.Id);
        
        builder.HasIndex(d => new { d.EmployerId, d.Id });
        builder.HasIndex(d => new { d.EmployerId, d.Name });
        
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(500);
        
        builder.HasOne(d => d.Employer)
            .WithMany(e => e.Departments)
            .HasForeignKey(d => d.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(d => d.Manager)
            .WithMany()
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}