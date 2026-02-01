using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Core.Entities.Audit;

public class TaxBracket : BaseEntity
{
    public int Id { get; set; }
    public int? EmployerId { get; set; }
    public string TaxType { get; set; }
    public int Year { get; set; }
    public string FilingStatus { get; set; }
    public decimal IncomeMin { get; set; }
    public decimal IncomeMax { get; set; }
    public decimal TaxRate { get; set; }
    public decimal FixedAmount { get; set; }
    public string State { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual Employer Employer { get; set; }
}