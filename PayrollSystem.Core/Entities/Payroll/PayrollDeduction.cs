using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Payroll;

public class PayrollDeduction : TenantEntity
{
    public int Id { get; set; }
    public int PayrollTransactionId { get; set; }
    public int DeductionTypeId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }

    // Navigation properties
    public virtual PayrollTransaction PayrollTransaction { get; set; }
    public virtual DeductionType DeductionType { get; set; }
}