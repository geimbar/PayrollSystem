using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.Leaves;

public class EmployeeLeaveBalance : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Year { get; set; }
    public decimal AccruedHours { get; set; }
    public decimal UsedHours { get; set; }
    public decimal RemainingHours { get; set; }
    public decimal CarryOverHours { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
    public virtual LeaveType LeaveType { get; set; }
}