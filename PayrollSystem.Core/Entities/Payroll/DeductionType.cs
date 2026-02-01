using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Payroll;

public class DeductionType : TenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DeductionCategory { get; set; }
    public bool IsPercentage { get; set; }
    public decimal? DefaultAmount { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual ICollection<EmployeeDeduction> EmployeeDeductions { get; set; }
}