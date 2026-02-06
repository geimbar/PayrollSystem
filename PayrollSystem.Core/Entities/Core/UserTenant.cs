namespace PayrollSystem.Core.Entities.Core;

/// <summary>
/// Junction table for users who can access multiple tenants
/// Can optionally be scoped to specific companies within a tenant
/// </summary>
public class UserTenant
{
    public string UserId { get; set; } = string.Empty;
    public Identity.ApplicationUser User { get; set; } = null!;

    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Optional: Limit access to specific company within tenant
    /// If null, user can access all companies in the tenant
    /// </summary>
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }

    /// <summary>
    /// Role within this tenant/company
    /// Examples: TenantAdmin, CompanyAdmin, Manager, User
    /// </summary>
    public string Role { get; set; } = "User";

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}
