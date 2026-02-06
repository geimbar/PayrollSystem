using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Leaves;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class EmployeeLeaveBalanceConfiguration : IEntityTypeConfiguration<EmployeeLeaveBalance>
{
    public void Configure(EntityTypeBuilder<EmployeeLeaveBalance> builder)
    {
        builder.ToTable("EmployeeLeaveBalances");
        builder.HasKey(lb => lb.Id);

        builder.HasIndex(lb => new { lb.EmployerId, lb.EmployeeId, lb.Year });

        builder.Property(lb => lb.AccruedHours).HasColumnType("decimal(8,2)");
        builder.Property(lb => lb.UsedHours).HasColumnType("decimal(8,2)");
        builder.Property(lb => lb.RemainingHours).HasColumnType("decimal(8,2)");
        builder.Property(lb => lb.CarryOverHours).HasColumnType("decimal(8,2)");

        builder.HasOne(lb => lb.Employer)
            .WithMany()
            .HasForeignKey(lb => lb.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lb => lb.Employee)
            .WithMany(e => e.LeaveBalances)
            .HasForeignKey(lb => lb.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lb => lb.LeaveType)
            .WithMany(lt => lt.EmployeeLeaveBalances)
            .HasForeignKey(lb => lb.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}