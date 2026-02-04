using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PayrollSystem.Core.Entities;
using PayrollSystem.Core.Interfaces;
using PayrollSystem.Infrastructure.Data;
using PayrollSystem.Infrastructure.Data.Seeder;
using PayrollSystem.Infrastructure.Services;
using PayrollSystem.Web.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["AppUrl"]!)
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Register scoped services
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();

// Register AccountService
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Add authentication state provider for Blazor
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Configure Identity with ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false; // Set to true in production
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

// Configure cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // Applies pending migrations
}

// Seed database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var seeder = new DataSeeder(context);

    // Seed employer, departments, employees
    await seeder.SeedAsync();

    // Seed demo user (requires employer to exist first)
    await seeder.SeedDemoUserAsync(userManager);

    // Optional: Seed additional test users
    // await seeder.SeedAdditionalUsersAsync(userManager);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseTenantMiddleware();

// If antiforgery is on, either handle token or keep auth endpoints before it while testing
app.UseAntiforgery();

//app.MapPost("/auth/login", async (HttpContext ctx, LoginRequest req, IAccountService accounts) =>
//{
//    try
//    {
//        var (success, error) = await accounts.LoginAsync(req.Email, req.Password, req.EmployerId);
//        return success ? Results.NoContent() : Results.BadRequest(new { error });
//    }
//    catch (OperationCanceledException) when (ctx.RequestAborted.IsCancellationRequested)
//    {
//        // client disconnected; ignore
//        return Results.NoContent();
//    }
//});

//app.MapPost("/auth/logout", async (IAccountService accountService) =>
//{
//    await accountService.LogoutAsync();
//    return Results.Ok();
//});

app.MapRazorComponents<PayrollSystem.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();

public record LoginRequest(string Email, string Password, int? EmployerId);