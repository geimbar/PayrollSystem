using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Core.Entities.Base;

public abstract class TenantEntity : ISoftDelete, IAuditable
{
    public int EmployerId { get; set; }
    public virtual Employer Employer { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}