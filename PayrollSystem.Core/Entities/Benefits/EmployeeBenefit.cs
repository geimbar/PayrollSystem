using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.Benefits;

public class EmployeeBenefit : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int BenefitPlanId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public decimal? EmployeeContribution { get; set; }
    public string CoverageLevel { get; set; }
    public string Status { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
    public virtual BenefitPlan BenefitPlan { get; set; }
}