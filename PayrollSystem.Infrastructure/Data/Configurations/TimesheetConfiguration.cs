using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.TimeTracking;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class TimeSheetConfiguration : IEntityTypeConfiguration<TimeSheet>
{
    public void Configure(EntityTypeBuilder<TimeSheet> builder)
    {
        builder.ToTable("TimeSheets");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => new { t.EmployerId, t.EmployeeId, t.Date });
        builder.HasIndex(t => new { t.EmployerId, t.Status });

        builder.Property(t => t.TotalHours).HasColumnType("decimal(5,2)");
        builder.Property(t => t.RegularHours).HasColumnType("decimal(5,2)");
        builder.Property(t => t.OvertimeHours).HasColumnType("decimal(5,2)");
        builder.Property(t => t.BreakHours).HasColumnType("decimal(5,2)");
        builder.Property(t => t.Status).HasMaxLength(50);
        builder.Property(t => t.Notes).HasMaxLength(1000);

        builder.HasOne(t => t.Employer)
            .WithMany()
            .HasForeignKey(t => t.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee who owns the timesheet
        builder.HasOne(t => t.Employee)
            .WithMany(e => e.TimeSheets)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee who approved
        builder.HasOne(t => t.ApprovedBy)
            .WithMany() // No navigation property back
            .HasForeignKey(t => t.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}