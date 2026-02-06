using Microsoft.AspNetCore.Http;
using PayrollSystem.Infrastructure.Services;

namespace PayrollSystem.Web.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantIdClaim = context.User.FindFirst("TenantId");
            var companyIdClaim = context.User.FindFirst("CompanyId");

            if (tenantIdClaim != null && int.TryParse(tenantIdClaim.Value, out int tenantId))
            {
                int? companyId = null;
                if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int cId))
                {
                    companyId = cId;
                }

                tenantService.SetCurrentTenant(tenantId, companyId);
            }
        }

        await _next(context);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}