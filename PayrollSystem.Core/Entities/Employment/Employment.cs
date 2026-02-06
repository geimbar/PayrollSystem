namespace PayrollSystem.Core.Entities.Employment;

/// <summary>
/// Employment - Links an Employee to a Company with specific job details
/// An employee can have multiple employments (jobs) in different companies
/// </summary>
public class Employment
{
    public int Id { get; set; }
    
    // Employee (Person)
    public int EmployeeId { get; set; }
    public People.Employee Employee { get; set; } = null!;
    
    // Company (Where they work)
    public int CompanyId { get; set; }
    public Core.Company Company { get; set; } = null!;
    
    // Department in this company
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    
    // Job Details
    public string JobTitle { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = "Full-time"; // Full-time, Part-time, Contract, Board, Consultant
    public string EmploymentStatus { get; set; } = "Active"; // Active, Terminated, On Leave, Suspended
    
    // Dates
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }
    
    // Work Schedule
    public decimal HoursPerWeek { get; set; } = 40;
    public string WorkLocation { get; set; } = string.Empty; // Office, Remote, Hybrid
    
    // Compensation
    public decimal BaseSalary { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentFrequency { get; set; } = "Monthly"; // Monthly, BiWeekly, Weekly, Hourly
    
    // Manager
    public int? ManagerEmploymentId { get; set; }
    public Employment? ManagerEmployment { get; set; }
    
    // For tax and payroll
    public string TaxCountry { get; set; } = string.Empty;
    public string TaxState { get; set; } = string.Empty;
    
    public bool IsPrimaryJob { get; set; } = true; // Is this their main job?
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public ICollection<Compensation> Compensations { get; set; } = new List<Compensation>();
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
}
