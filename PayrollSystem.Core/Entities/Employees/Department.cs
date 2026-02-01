using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Core.Entities.Employees;

public class Department : TenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ManagerId { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual Employee Manager { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
}