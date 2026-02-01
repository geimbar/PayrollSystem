using PayrollSystem.Core.Interfaces;

namespace PayrollSystem.Infrastructure.Services;

public class TenantService : ITenantService
{
    private int _currentEmployerId;

    public int GetCurrentEmployerId()
    {
        if (_currentEmployerId == 0)
        {
            throw new InvalidOperationException(
                "EmployerId has not been set. Ensure TenantMiddleware is configured.");
        }
        return _currentEmployerId;
    }

    public void SetCurrentEmployer(int employerId)
    {
        if (employerId <= 0)
        {
            throw new ArgumentException("EmployerId must be greater than 0", nameof(employerId));
        }
        _currentEmployerId = employerId;
    }

    public bool HasAccess(int employerId)
    {
        return _currentEmployerId == employerId;
    }
}