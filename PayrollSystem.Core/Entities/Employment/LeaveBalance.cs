namespace PayrollSystem.Core.Entities.Employment;

/// <summary>
/// Leave Balance - Vacation/Sick days per employment
/// </summary>
public class LeaveBalance
{
    public int Id { get; set; }
    
    public int EmploymentId { get; set; }
    public Employment Employment { get; set; } = null!;
    
    public string LeaveType { get; set; } = string.Empty; // Vacation, Sick, Personal, Parental
    public decimal TotalDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal RemainingDays { get; set; }
    
    public int Year { get; set; } = DateTime.UtcNow.Year;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
