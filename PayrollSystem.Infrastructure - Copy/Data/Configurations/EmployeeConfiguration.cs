using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Employees;
using PayrollSystem.Core.Interfaces;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.Id);
        
        builder.HasIndex(e => new { e.EmployerId, e.Id });
        builder.HasIndex(e => new { e.EmployerId, e.Email });
        builder.HasIndex(e => new { e.EmployerId, e.EmployeeNumber });
        builder.HasIndex(e => e.DepartmentId);
        
        builder.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(50);
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.MiddleName).HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.JobTitle).HasMaxLength(100);
        builder.Property(e => e.EmploymentType).HasMaxLength(50);
        builder.Property(e => e.EmploymentStatus).HasMaxLength(50);
        
        // Note: Encryption is handled in a custom value converter
        // For now, we'll store SSN as-is and add encryption later
        builder.Property(e => e.SSN).HasMaxLength(500);
        
        builder.Property(e => e.RowVersion).IsRowVersion();
        
        builder.HasOne(e => e.Employer)
            .WithMany(emp => emp.Employees)
            .HasForeignKey(e => e.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}