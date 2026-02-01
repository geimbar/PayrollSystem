using Microsoft.AspNetCore.Identity;
using PayrollSystem.Core.Entities.Employers;
using PayrollSystem.Core.Entities.Audit;

namespace PayrollSystem.Core.Entities.System;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    // Navigation properties
    public virtual ICollection<EmployerUser> EmployerUsers { get; set; }
    public virtual ICollection<AuditLog> AuditLogs { get; set; }
}