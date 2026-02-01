using PayrollSystem.Core.Entities.Employers;
using PayrollSystem.Core.Entities.System;

namespace PayrollSystem.Core.Entities.Audit;

public class AuditLog
{
    public int Id { get; set; }
    public int? EmployerId { get; set; }
    public string UserId { get; set; }
    public string EntityName { get; set; }
    public string EntityId { get; set; }
    public string Action { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public DateTime Timestamp { get; set; }

    // Navigation properties
    public virtual Employer Employer { get; set; }
    public virtual User User { get; set; }
}