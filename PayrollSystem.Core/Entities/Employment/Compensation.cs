namespace PayrollSystem.Core.Entities.Employment;

/// <summary>
/// Compensation - Additional pay components for an employment
/// </summary>
public class Compensation
{
    public int Id { get; set; }
    
    public int EmploymentId { get; set; }
    public Employment Employment { get; set; } = null!;
    
    public string CompensationType { get; set; } = string.Empty; // Bonus, Commission, Allowance, Stock
    public string Description { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Frequency { get; set; } = "OneTime"; // OneTime, Monthly, Quarterly, Annually
    
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public bool IsRecurring { get; set; } = false;
    public bool IsTaxable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
