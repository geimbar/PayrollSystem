# Sprint 2 - Quick Start Guide

## 🚀 Get Authentication Working in 30 Minutes

### Prerequisites
- Sprint 1 completed ✅
- Database running ✅
- .NET 8 SDK installed ✅

---

## Fast Track Implementation

### Step 1: Copy Files (5 minutes)

```bash
# Copy Core entities
cp ApplicationUser.cs → PayrollSystem.Core/Entities/

# Copy Infrastructure files
cp UserEmployerConfiguration.cs → PayrollSystem.Infrastructure/Data/Configurations/
cp AccountService.cs → PayrollSystem.Infrastructure/Services/

# Copy Web files
cp TenantMiddleware.cs → PayrollSystem.Web/Middleware/
cp EmptyLayout.razor → PayrollSystem.Web/Layouts/
cp Login.razor → PayrollSystem.Web/Pages/Account/
cp Logout.razor → PayrollSystem.Web/Pages/Account/
```

### Step 2: Update DbContext (3 minutes)

Add to `ApplicationDbContext.cs`:
```csharp
public DbSet<ApplicationUser> ApplicationUsers { get; set; }
public DbSet<UserEmployer> UserEmployers { get; set; }

// In OnModelCreating:
modelBuilder.Entity<ApplicationUser>()
    .HasOne(u => u.PrimaryEmployer)
    .WithMany()
    .HasForeignKey(u => u.PrimaryEmployerId)
    .OnDelete(DeleteBehavior.SetNull);
```

### Step 3: Update Program.cs (5 minutes)

Follow the `Program.cs.instructions.txt` file to:
1. Update Identity configuration
2. Register AccountService
3. Add TenantMiddleware
4. Configure cookies

### Step 4: Database Migration (2 minutes)

```bash
dotnet ef migrations add "AddAuthenticationEntities" \
    --project PayrollSystem.Infrastructure \
    --startup-project PayrollSystem.Web

dotnet ef database update \
    --project PayrollSystem.Infrastructure \
    --startup-project PayrollSystem.Web
```

### Step 5: Seed Demo User (5 minutes)

Add to `DataSeeder.cs` and update Program.cs (see DATABASE_MIGRATION_GUIDE.md)

### Step 6: Test (10 minutes)

```bash
dotnet run --project PayrollSystem.Web
```

Navigate to: http://localhost:5000/login

**Login with:**
- Email: `admin@democompany.com`
- Password: `Admin123!`

---

## Expected Results

✅ Login page displays with MudBlazor styling
✅ Can enter credentials and submit
✅ Employer selection appears (if multiple employers)
✅ Redirects to home page after login
✅ User menu shows in top right
✅ Can logout successfully
✅ Protected pages require authentication

---

## Troubleshooting

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

### Migration Errors
```bash
# Check migration status
dotnet ef migrations list --project PayrollSystem.Infrastructure --startup-project PayrollSystem.Web

# Remove last migration if needed
dotnet ef migrations remove --project PayrollSystem.Infrastructure --startup-project PayrollSystem.Web
```

### Login Loop
- Check [AllowAnonymous] attribute on Login.razor
- Verify middleware order in Program.cs
- Check cookie configuration

### No Data Showing
- Verify demo data seeding ran
- Check TenantService is setting EmployerId
- Add logging to TenantMiddleware

---

## File Structure

```
PayrollSystem/
├── PayrollSystem.Core/
│   └── Entities/
│       ├── ApplicationUser.cs ← NEW
│       └── UserEmployer.cs ← NEW (in same file)
├── PayrollSystem.Infrastructure/
│   ├── Data/
│   │   └── Configurations/
│   │       └── UserEmployerConfiguration.cs ← NEW
│   └── Services/
│       └── AccountService.cs ← NEW
└── PayrollSystem.Web/
    ├── Middleware/
    │   └── TenantMiddleware.cs ← NEW
    ├── Layouts/
    │   ├── EmptyLayout.razor ← NEW
    │   └── MainLayout.razor ← UPDATE
    └── Pages/
        └── Account/
            ├── Login.razor ← NEW
            └── Logout.razor ← NEW
```

---

## Next Phase

Once authentication works, move to Phase 2:
- Employee CRUD pages
- Department management
- Advanced authorization

---

## Support

If stuck:
1. Check IMPLEMENTATION_CHECKLIST.md for detailed steps
2. Review DATABASE_MIGRATION_GUIDE.md for seeding help
3. Check Program.cs.instructions.txt for configuration
4. Review error messages carefully
5. Check database for demo user existence

---

**Time to Complete**: ~30 minutes
**Difficulty**: Medium
**Prerequisites**: Sprint 1 complete
