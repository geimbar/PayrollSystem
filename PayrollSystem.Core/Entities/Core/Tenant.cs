using PayrollSystem.Core.Entities.People;

namespace PayrollSystem.Core.Entities.Core;

/// <summary>
/// Top-level organization (e.g., "SHS Group")
/// </summary>
public class Tenant
{
    public int Id { get; set; }
    
    public string TenantName { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    
    // Contact Information
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public ICollection<Company> Companies { get; set; } = new List<Company>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}
