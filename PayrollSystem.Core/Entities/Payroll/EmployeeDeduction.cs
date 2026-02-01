using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.Payroll;

public class EmployeeDeduction : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int DeductionTypeId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
    public virtual DeductionType DeductionType { get; set; }
}