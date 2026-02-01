namespace PayrollSystem.Core.Entities.Base;

public abstract class BaseEntity : ISoftDelete, IAuditable
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}