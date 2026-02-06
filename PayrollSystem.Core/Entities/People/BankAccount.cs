namespace PayrollSystem.Core.Entities.People;

/// <summary>
/// Bank Account - Employee's payment details (Encrypted)
/// </summary>
public class BankAccount
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    
    public string BankName { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty; // Encrypted
    public string RoutingNumber { get; set; } = string.Empty; // Encrypted
    public string IBAN { get; set; } = string.Empty; // Encrypted
    public string SWIFT { get; set; } = string.Empty;
    
    public string AccountType { get; set; } = "Checking"; // Checking, Savings
    public string Currency { get; set; } = "USD";
    
    public bool IsPrimaryAccount { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
