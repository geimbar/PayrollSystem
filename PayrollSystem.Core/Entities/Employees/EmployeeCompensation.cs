using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Employees;

public class EmployeeCompensation : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string CompensationType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PayFrequency { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
}