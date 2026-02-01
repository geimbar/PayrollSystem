using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.TimeTracking;

public class TimeSheet : TenantEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public DateTime? ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public decimal TotalHours { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal BreakHours { get; set; }
    public string Status { get; set; }
    public int? ApprovedById { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string Notes { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; }
    public virtual Employee ApprovedBy { get; set; }
}