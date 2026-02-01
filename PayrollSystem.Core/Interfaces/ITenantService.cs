namespace PayrollSystem.Core.Interfaces;

public interface ITenantService
{
    int GetCurrentEmployerId();
    void SetCurrentEmployer(int employerId);
    bool HasAccess(int employerId);
}