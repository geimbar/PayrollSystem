# Sprint 2 Database Migration Guide

## Step 1: Update ApplicationDbContext.cs

Add these DbSets to your ApplicationDbContext:

```csharp
// In ApplicationDbContext.cs, add:
public DbSet<ApplicationUser> ApplicationUsers { get; set; }
public DbSet<UserEmployer> UserEmployers { get; set; }

// In OnModelCreating method, add:
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Apply all configurations
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    
    // Configure ApplicationUser relationships
    modelBuilder.Entity<ApplicationUser>()
        .HasOne(u => u.PrimaryEmployer)
        .WithMany()
        .HasForeignKey(u => u.PrimaryEmployerId)
        .OnDelete(DeleteBehavior.SetNull);

    // ... existing code for global filters ...
}
```

## Step 2: Create Migration

```bash
# From solution root directory:
dotnet ef migrations add "AddAuthenticationEntities" \
    --project PayrollSystem.Infrastructure \
    --startup-project PayrollSystem.Web
```

## Step 3: Update Database

```bash
dotnet ef database update \
    --project PayrollSystem.Infrastructure \
    --startup-project PayrollSystem.Web
```

## Step 4: Seed Demo User (Optional)

Add this to your DataSeeder.cs to create a test user:

```csharp
public async Task SeedDemoUserAsync(UserManager<ApplicationUser> userManager)
{
    // Check if demo user exists
    var demoUser = await userManager.FindByEmailAsync("admin@democompany.com");
    
    if (demoUser == null)
    {
        // Get the demo employer
        var demoEmployer = await _context.Employers
            .FirstOrDefaultAsync(e => e.CompanyName == "Demo Company Inc.");

        if (demoEmployer == null)
        {
            throw new Exception("Demo employer not found. Run initial seed first.");
        }

        // Create demo admin user
        demoUser = new ApplicationUser
        {
            UserName = "admin@democompany.com",
            Email = "admin@democompany.com",
            EmailConfirmed = true,
            FullName = "System Administrator",
            PrimaryEmployerId = demoEmployer.Id,
            IsSystemAdmin = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(demoUser, "Admin123!");

        if (result.Succeeded)
        {
            // Add user-employer relationship
            _context.UserEmployers.Add(new UserEmployer
            {
                UserId = demoUser.Id,
                EmployerId = demoEmployer.Id,
                Role = "Admin",
                GrantedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            
            Console.WriteLine("Demo user created:");
            Console.WriteLine("Email: admin@democompany.com");
            Console.WriteLine("Password: Admin123!");
        }
        else
        {
            Console.WriteLine("Failed to create demo user:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
}
```

## Step 5: Update Program.cs to seed user

In Program.cs, after existing seeding code:

```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    
    var seeder = new DataSeeder(context);
    await seeder.SeedAsync();
    await seeder.SeedDemoUserAsync(userManager); // Add this line
}
```

## Test Login Credentials

After seeding:
- Email: `admin@democompany.com`
- Password: `Admin123!`
- Company: Demo Company Inc.
