namespace PayrollSystem.Infrastructure.Services;

public interface ITenantService
{
    int? GetCurrentTenantId();
    int? GetCurrentCompanyId();
    void SetCurrentTenant(int tenantId, int? companyId = null);
}

public class TenantService : ITenantService
{
    private int? _currentTenantId;
    private int? _currentCompanyId;

    public int? GetCurrentTenantId() => _currentTenantId;
    public int? GetCurrentCompanyId() => _currentCompanyId;

    public void SetCurrentTenant(int tenantId, int? companyId = null)
    {
        _currentTenantId = tenantId;
        _currentCompanyId = companyId;
    }
}