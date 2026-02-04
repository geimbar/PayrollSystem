using PayrollSystem.Infrastructure.Services;
using System.Security.Claims;

namespace PayrollSystem.Web.Middleware;

/// <summary>
/// Middleware that automatically sets the current employer context from user claims
/// </summary>
public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, TenantService tenantService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // Try to get EmployerId from claims
            var employerIdClaim = context.User.FindFirst("EmployerId");
            
            if (employerIdClaim != null && int.TryParse(employerIdClaim.Value, out int employerId))
            {
                tenantService.SetCurrentEmployer(employerId);
                _logger.LogDebug("Tenant context set to EmployerId: {EmployerId} for user: {UserName}", 
                    employerId, context.User.Identity.Name);
            }
            else
            {
                // Check if user is system admin
                var isSystemAdmin = context.User.HasClaim("IsSystemAdmin", "true");
                
                if (!isSystemAdmin)
                {
                    _logger.LogWarning("Authenticated user {UserName} has no EmployerId claim and is not system admin", 
                        context.User.Identity.Name);
                }
            }
        }

        await _next(context);
    }
}

/// <summary>
/// Extension method for registering the tenant middleware
/// </summary>
public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}
