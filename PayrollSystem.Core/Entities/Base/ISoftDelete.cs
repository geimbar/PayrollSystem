namespace PayrollSystem.Core.Entities.Base;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}