namespace PayrollSystem.Core.Entities.People;

/// <summary>
/// Employee - Person information at tenant level
/// Can have multiple jobs (employments) in different companies
/// </summary>
public class Employee
{
    public int Id { get; set; }
    
    // Belongs to Tenant (not Company!)
    public int TenantId { get; set; }
    public Core.Tenant Tenant { get; set; } = null!;
    
    // Personal Information
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PreferredName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string PersonalEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    
    // Identification (Encrypted)
    public string SSN { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    
    // Address
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    
    // Emergency Contact
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;
    public string EmergencyContactRelationship { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public ICollection<Employment.Employment> Employments { get; set; } = new List<Employment.Employment>();
    public ICollection<Tax.TaxCard> TaxCards { get; set; } = new List<Tax.TaxCard>();
    public ICollection<NextOfKin> NextOfKins { get; set; } = new List<NextOfKin>();
    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
