using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Employees;

public class EmployeeBankAccount : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string BankName { get; set; }
    public string AccountNumber { get; set; }
    public string RoutingNumber { get; set; }
    public string AccountType { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
}