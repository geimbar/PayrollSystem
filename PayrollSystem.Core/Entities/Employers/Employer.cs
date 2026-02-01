using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.Employees;

namespace PayrollSystem.Core.Entities.Employers;

public class Employer : BaseEntity
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string TaxIdentificationNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string? Website { get; set; }

    // Branding
    public string? LogoUrl { get; set; }
    public string? BrandingColorPrimary { get; set; }
    public string? BrandingColorSecondary { get; set; }

    // Subscription
    public string SubscriptionPlan { get; set; }
    public string SubscriptionStatus { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }

    public bool IsActive { get; set; }

    // Concurrency token
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual EmployerSettings Settings { get; set; }
    public virtual ICollection<EmployerUser> EmployerUsers { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<Department> Departments { get; set; }
}