using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PayrollSystem.Core.Entities;
using PayrollSystem.Infrastructure.Data;
using System.Security.Claims;

namespace PayrollSystem.Infrastructure.Services;

public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    private readonly ApplicationDbContext _context;

    public CustomUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ApplicationDbContext context)
        : base(userManager, roleManager, options)
    {
        _context = context;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        // Add custom claims
        identity.AddClaim(new Claim("FullName", user.FullName));

        if (user.IsSystemAdmin)
        {
            identity.AddClaim(new Claim("IsSystemAdmin", "true"));
        }

        // Add primary employer ID
        if (user.PrimaryEmployerId.HasValue)
        {
            identity.AddClaim(new Claim("EmployerId", user.PrimaryEmployerId.Value.ToString()));
        }
        else if (user.IsSystemAdmin)
        {
            // For system admin, get first employer
            var firstEmployer = await _context.Employers.FirstOrDefaultAsync();
            if (firstEmployer != null)
            {
                identity.AddClaim(new Claim("EmployerId", firstEmployer.Id.ToString()));
            }
        }

        // Add role for the user's primary employer
        if (!user.IsSystemAdmin && user.PrimaryEmployerId.HasValue)
        {
            var userEmployer = await _context.UserEmployers
                .FirstOrDefaultAsync(ue => ue.UserId == user.Id && ue.EmployerId == user.PrimaryEmployerId.Value);

            if (userEmployer != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, userEmployer.Role));
            }
        }
        else if (user.IsSystemAdmin)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "SystemAdmin"));
        }

        return identity;
    }
}
