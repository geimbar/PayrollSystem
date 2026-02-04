using Microsoft.AspNetCore.Identity;
using PayrollSystem.Core.Entities.Employers;

namespace PayrollSystem.Core.Entities;

/// <summary>
/// Extended Identity user with multi-tenant support
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Primary employer for this user (for single-tenant users)
    /// </summary>
    public int? PrimaryEmployerId { get; set; }
    public Employer? PrimaryEmployer { get; set; }

    /// <summary>
    /// All employers this user has access to (for multi-tenant admins)
    /// </summary>
    public ICollection<UserEmployer> UserEmployers { get; set; } = new List<UserEmployer>();

    /// <summary>
    /// Is this a system administrator (access to all tenants)?
    /// </summary>
    public bool IsSystemAdmin { get; set; }

    /// <summary>
    /// Account creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last login timestamp
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Is the account active?
    /// </summary>
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Junction table for users who can access multiple employers
/// </summary>
public class UserEmployer
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int EmployerId { get; set; }
    public Employer Employer { get; set; } = null!;

    /// <summary>
    /// Role within this specific employer (Admin, Manager, User, etc.)
    /// </summary>
    public string Role { get; set; } = "User";

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}
