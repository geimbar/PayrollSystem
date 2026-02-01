using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Benefits;

public class BenefitPlan : TenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string BenefitType { get; set; }
    public string ProviderName { get; set; }
    public decimal? EmployerContribution { get; set; }
    public decimal? EmployeeContribution { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual ICollection<EmployeeBenefit> EmployeeBenefits { get; set; }
}