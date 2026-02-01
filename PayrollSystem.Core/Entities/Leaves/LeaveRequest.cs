using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.Leaves;

public class LeaveRequest : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalHours { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public int? ApprovedById { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string ApproverComments { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
    public virtual LeaveType LeaveType { get; set; }
    public virtual Employee ApprovedBy { get; set; }
}