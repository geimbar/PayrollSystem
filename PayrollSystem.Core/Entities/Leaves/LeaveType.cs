using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Leaves;

public class LeaveType : TenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPaid { get; set; }
    public decimal? DefaultDaysPerYear { get; set; }
    public decimal? AccrualRate { get; set; }
    public decimal? MaxCarryOver { get; set; }
    public bool RequiresApproval { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual ICollection<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; }
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
}