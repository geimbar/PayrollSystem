using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayrollSystem.Core.Entities;
using PayrollSystem.Infrastructure.Data;
using System.Security.Claims;

namespace PayrollSystem.Infrastructure.Services;

public interface IAccountService
{
    Task<(bool Success, string ErrorMessage)> LoginAsync(string email, string password, int? employerId = null);
    Task LogoutAsync();
    Task<List<EmployerSelectionDto>> GetUserEmployersAsync(string email);
    Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal principal);
}

public class AccountService : IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccountService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger<AccountService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<EmployerSelectionDto>> GetUserEmployersAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new List<EmployerSelectionDto>();

        // System admins can see all employers
        if (user.IsSystemAdmin)
        {
            return await _context.Employers
                .Where(e => e.IsActive)
                .Select(e => new EmployerSelectionDto
                {
                    EmployerId = e.Id,
                    CompanyName = e.CompanyName,
                    Role = "System Admin"
                })
                .ToListAsync();
        }

        // Regular users see only their assigned employers
        var employers = await _context.UserEmployers
            .Where(ue => ue.UserId == user.Id)
            .Include(ue => ue.Employer)
            .Where(ue => ue.Employer.IsActive)
            .Select(ue => new EmployerSelectionDto
            {
                EmployerId = ue.EmployerId,
                CompanyName = ue.Employer.CompanyName,
                Role = ue.Role
            })
            .ToListAsync();

        // Add primary employer if set and not already in list
        if (user.PrimaryEmployerId.HasValue && !employers.Any(e => e.EmployerId == user.PrimaryEmployerId.Value))
        {
            var primaryEmployer = await _context.Employers
                .Where(e => e.Id == user.PrimaryEmployerId.Value && e.IsActive)
                .FirstOrDefaultAsync();

            if (primaryEmployer != null)
            {
                employers.Insert(0, new EmployerSelectionDto
                {
                    EmployerId = primaryEmployer.Id,
                    CompanyName = primaryEmployer.CompanyName,
                    Role = "Primary"
                });
            }
        }

        return employers;
    }

    public async Task<(bool Success, string ErrorMessage)> LoginAsync(string email, string password, int? employerId = null)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("Login attempt with invalid email: {Email}", email);
            return (false, "Invalid email or password");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login attempt for inactive user: {Email}", email);
            return (false, "Account is inactive. Please contact your administrator.");
        }

        // Determine which employer to use
        int selectedEmployerId;

        if (employerId.HasValue)
        {
            // Verify user has access to this employer
            if (!user.IsSystemAdmin)
            {
                var hasAccess = await _context.UserEmployers
                    .AnyAsync(ue => ue.UserId == user.Id && ue.EmployerId == employerId.Value);

                if (!hasAccess && user.PrimaryEmployerId != employerId.Value)
                {
                    return (false, "You don't have access to this employer.");
                }
            }

            selectedEmployerId = employerId.Value;

            // Update user's primary employer for this session
            user.PrimaryEmployerId = selectedEmployerId;
            await _userManager.UpdateAsync(user);
        }
        else if (user.PrimaryEmployerId.HasValue)
        {
            selectedEmployerId = user.PrimaryEmployerId.Value;
        }
        else if (user.IsSystemAdmin)
        {
            // System admin with no specific employer selected
            var firstEmployer = await _context.Employers.FirstOrDefaultAsync();
            if (firstEmployer == null)
            {
                return (false, "No employers found in the system.");
            }
            selectedEmployerId = firstEmployer.Id;
            user.PrimaryEmployerId = selectedEmployerId;
            await _userManager.UpdateAsync(user);
        }
        else
        {
            return (false, "No employer associated with this account.");
        }

        // Check if we have an HttpContext (needed for cookie authentication)
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            _logger.LogError("HttpContext is null during login attempt");
            return (false, "Authentication system error. Please try again.");
        }

        // Check if response has already started
        if (httpContext.Response.HasStarted)
        {
            _logger.LogError("Response has already started, cannot set authentication cookie");
            return (false, "Authentication timing error. Please refresh the page and try again.");
        }

        // Simple password sign-in
        var result = await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return (false, "Account is locked out due to multiple failed login attempts.");
            if (result.IsNotAllowed)
                return (false, "Login not allowed. Please verify your email.");

            return (false, "Invalid email or password");
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("User {Email} logged in successfully with EmployerId: {EmployerId}",
            email, selectedEmployerId);

        return (true, string.Empty);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        return await _userManager.GetUserAsync(principal);
    }
}

public class EmployerSelectionDto
{
    public int EmployerId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
