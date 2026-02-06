namespace PayrollSystem.Core.Entities.Core;

/// <summary>
/// Company/Financial entity within a tenant (e.g., "Global", "Europe", "Development")
/// </summary>
public class Company
{
    public int Id { get; set; }
    
    // Belongs to Tenant
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public string CompanyName { get; set; } = string.Empty;
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
    public ICollection<Employment.Department> Departments { get; set; } = new List<Employment.Department>();
    public ICollection<Employment.Employment> Employments { get; set; } = new List<Employment.Employment>();
}
