using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Payroll;

public class PayrollPeriod : TenantEntity
{
    public int Id { get; set; }
    public string PeriodType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PayDate { get; set; }
    public string Status { get; set; }

    // Navigation properties
    public virtual ICollection<PayrollTransaction> PayrollTransactions { get; set; }
}