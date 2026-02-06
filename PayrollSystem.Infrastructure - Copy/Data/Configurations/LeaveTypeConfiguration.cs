using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Leaves;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.ToTable("LeaveTypes");
        builder.HasKey(lt => lt.Id);

        builder.Property(lt => lt.Name).IsRequired().HasMaxLength(100);
        builder.Property(lt => lt.Description).HasMaxLength(500);
        builder.Property(lt => lt.DefaultDaysPerYear).HasColumnType("decimal(5,2)");
        builder.Property(lt => lt.AccrualRate).HasColumnType("decimal(5,2)");
        builder.Property(lt => lt.MaxCarryOver).HasColumnType("decimal(5,2)");

        builder.HasOne(lt => lt.Employer)
            .WithMany()
            .HasForeignKey(lt => lt.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}