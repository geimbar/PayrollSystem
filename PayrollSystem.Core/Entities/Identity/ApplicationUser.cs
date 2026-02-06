using Microsoft.AspNetCore.Identity;

namespace PayrollSystem.Core.Entities.Identity;

/// <summary>
/// Extended Identity user with tenant support
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Primary tenant for this user
    /// </summary>
    public int? PrimaryTenantId { get; set; }
    public Core.Tenant? PrimaryTenant { get; set; }

    /// <summary>
    /// All tenants this user has access to
    /// </summary>
    public ICollection<Core.UserTenant> UserTenants { get; set; } = new List<Core.UserTenant>();

    /// <summary>
    /// Is this a system administrator (access to all tenants)?
    /// </summary>
    public bool IsSystemAdmin { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
}
