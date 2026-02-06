namespace PayrollSystem.Core.Entities.Tax;

/// <summary>
/// Tax Card - Employee's tax information
/// </summary>
public class TaxCard
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public People.Employee Employee { get; set; } = null!;
    
    public string TaxCardNumber { get; set; } = string.Empty;
    public string TaxCountry { get; set; } = string.Empty;
    public string TaxState { get; set; } = string.Empty;
    
    public decimal TaxRate { get; set; }
    public decimal DeductionAmount { get; set; }
    
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
