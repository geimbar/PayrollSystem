using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollSystem.Core.Entities.Audit;

namespace PayrollSystem.Infrastructure.Data.Configurations;

public class TaxBracketConfiguration : IEntityTypeConfiguration<TaxBracket>
{
    public void Configure(EntityTypeBuilder<TaxBracket> builder)
    {
        builder.ToTable("TaxBrackets");
        builder.HasKey(tb => tb.Id);

        builder.Property(tb => tb.TaxType).IsRequired().HasMaxLength(50);
        builder.Property(tb => tb.FilingStatus).HasMaxLength(50);
        builder.Property(tb => tb.IncomeMin).HasColumnType("decimal(18,2)");
        builder.Property(tb => tb.IncomeMax).HasColumnType("decimal(18,2)");
        builder.Property(tb => tb.TaxRate).HasColumnType("decimal(5,4)");
        builder.Property(tb => tb.FixedAmount).HasColumnType("decimal(18,2)");
        builder.Property(tb => tb.State).HasMaxLength(2);

        builder.HasOne(tb => tb.Employer)
            .WithMany()
            .HasForeignKey(tb => tb.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}