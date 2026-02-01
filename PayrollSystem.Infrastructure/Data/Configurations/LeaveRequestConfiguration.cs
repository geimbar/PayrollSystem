using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Leaves;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests");
        builder.HasKey(lr => lr.Id);

        builder.HasIndex(lr => new { lr.EmployerId, lr.EmployeeId });
        builder.HasIndex(lr => new { lr.EmployerId, lr.Status });

        builder.Property(lr => lr.Status).HasMaxLength(50);
        builder.Property(lr => lr.Reason).HasMaxLength(1000);
        builder.Property(lr => lr.TotalHours).HasColumnType("decimal(8,2)");

        builder.HasOne(lr => lr.Employer)
            .WithMany()
            .HasForeignKey(lr => lr.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee who requested the leave
        builder.HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee who approved the leave
        builder.HasOne(lr => lr.ApprovedBy)
            .WithMany() // No navigation property back
            .HasForeignKey(lr => lr.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.LeaveType)
            .WithMany(lt => lt.LeaveRequests)
            .HasForeignKey(lr => lr.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}