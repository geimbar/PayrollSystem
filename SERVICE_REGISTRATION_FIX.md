# Service Registration Fix for TenantService

## 🔴 The Problem

```
Unable to resolve service for type 'PayrollSystem.Infrastructure.Services.TenantService'
```

The TenantService needs to be registered in the dependency injection container.

## ✅ Solution: Register TenantService in Program.cs

### Find This Section in Program.cs:

Look for where services are being registered (usually near the top, after `var builder = WebApplication.CreateBuilder(args);`)

### Add TenantService Registration:

```csharp
// Register TenantService as Scoped (one instance per request)
builder.Services.AddScoped<TenantService>();
```

## 📋 Complete Service Registration Section

Your Program.cs should have a section that looks like this:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor
builder.Services.AddMudServices();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// ⭐ INFRASTRUCTURE SERVICES - ADD THESE:
builder.Services.AddScoped<TenantService>();           // ← ADD THIS
builder.Services.AddScoped<IAccountService, AccountService>();  // ← ADD THIS
builder.Services.AddScoped<EncryptionService>();       // ← If you have this

// Authentication state
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();
```

## 🔍 Key Points:

### 1. TenantService Lifetime: `Scoped`
- **Scoped** = One instance per HTTP request
- Perfect for tenant context that should last for the entire request
- NOT Singleton (would share across all requests)
- NOT Transient (would create multiple instances per request)

### 2. Service Registration Order
The order of service registration doesn't usually matter, but for clarity:
1. Framework services (Razor, MudBlazor)
2. Database context
3. Identity
4. Your infrastructure services
5. Authentication state

## 🧪 Verify Registration

After adding the service registration:

1. **Build the solution**:
   ```bash
   dotnet build
   ```

2. **Run the application**:
   ```bash
   dotnet run --project PayrollSystem.Web
   ```

3. **Check for the error** - it should be gone!

## 📝 Complete Minimal Example

Here's a minimal Program.cs with all required registrations:

```csharp
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PayrollSystem.Core.Entities;
using PayrollSystem.Infrastructure.Data;
using PayrollSystem.Infrastructure.Services;
using PayrollSystem.Web.Components;
using PayrollSystem.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor
builder.Services.AddMudServices();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// ⭐ YOUR SERVICES:
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<EncryptionService>();

// Authentication
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();
app.UseTenantMiddleware();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Seeding (optional - for development)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    var seeder = new DataSeeder(context);
    await seeder.SeedAsync();
    await seeder.SeedDemoUserAsync(userManager);
}

app.Run();
```

## 🔍 Troubleshooting

### Still Getting the Error?

**Check 1:** Verify TenantService exists
- File: `PayrollSystem.Infrastructure/Services/TenantService.cs`
- Should be a class (not interface)

**Check 2:** Verify namespace
- TenantService should be in namespace: `PayrollSystem.Infrastructure.Services`
- The using statement in Program.cs should match

**Check 3:** Check if it's an interface
If your TenantService is actually an interface `ITenantService`, register it like:
```csharp
builder.Services.AddScoped<ITenantService, TenantService>();
```

**Check 4:** Verify TenantMiddleware signature
Your TenantMiddleware.InvokeAsync should accept TenantService (not ITenantService), like:
```csharp
public async Task InvokeAsync(HttpContext context, TenantService tenantService)
```

## 🎯 Expected Outcome

After adding the service registration:
- ✅ Application starts without DI error
- ✅ TenantMiddleware can resolve TenantService
- ✅ Login page loads
- ✅ Authentication works
- ✅ Tenant context is set after login

## 📚 Related Services

You might also need to register:
```csharp
builder.Services.AddScoped<TenantService>();          // Tenant context
builder.Services.AddScoped<IAccountService, AccountService>();  // Authentication
builder.Services.AddScoped<EncryptionService>();      // Data encryption (if used)
```

All infrastructure services should be registered before building the app (`var app = builder.Build();`).
