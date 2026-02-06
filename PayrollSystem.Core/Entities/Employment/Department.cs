namespace PayrollSystem.Core.Entities.Employment;

/// <summary>
/// Department within a Company
/// </summary>
public class Department
{
    public int Id { get; set; }
    
    // Belongs to Company
    public int CompanyId { get; set; }
    public Core.Company Company { get; set; } = null!;
    
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Cost Center for accounting
    public string CostCenter { get; set; } = string.Empty;
    
    // Parent Department (for hierarchy)
    public int? ParentDepartmentId { get; set; }
    public Department? ParentDepartment { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public ICollection<Employment> Employments { get; set; } = new List<Employment>();
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();
}
