namespace PayrollSystem.Core.Entities.People;

/// <summary>
/// Next of Kin - Emergency contact
/// </summary>
public class NextOfKin
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    public bool IsPrimaryContact { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
