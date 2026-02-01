using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Payroll;

public class PayrollTax : TenantEntity
{
    public int Id { get; set; }
    public int PayrollTransactionId { get; set; }
    public string TaxType { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public string Description { get; set; }

    // Navigation properties
    public virtual PayrollTransaction PayrollTransaction { get; set; }
}