using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.Payroll;

public class PayrollTransaction : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int PayrollPeriodId { get; set; }

    // Pay amounts
    public decimal GrossPay { get; set; }
    public decimal NetPay { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal RegularPay { get; set; }
    public decimal OvertimePay { get; set; }
    public decimal BonusPay { get; set; }
    public decimal CommissionPay { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalTaxes { get; set; }

    // Payment details
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string CheckNumber { get; set; }
    public string Notes { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
    public virtual PayrollPeriod PayrollPeriod { get; set; }
    public virtual ICollection<PayrollDeduction> Deductions { get; set; }
    public virtual ICollection<PayrollTax> Taxes { get; set; }
}