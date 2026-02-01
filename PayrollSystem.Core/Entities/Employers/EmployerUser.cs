using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.System;

namespace PayrollSystem.Core.Entities.Employers;

public class EmployerUser : BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int EmployerId { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Employer Employer { get; set; }
}