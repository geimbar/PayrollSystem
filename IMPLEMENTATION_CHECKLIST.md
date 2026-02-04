# Sprint 2 - Phase 1: Authentication Implementation Checklist

## Overview
This checklist covers implementing multi-tenant authentication with employer selection for your Payroll System.

---

## Phase 1A: Entity Updates ✅

### 1. Update Core Entities
- [ ] Copy `ApplicationUser.cs` to `PayrollSystem.Core/Entities/`
- [ ] Update `Employer.cs` to include navigation properties if needed
- [ ] Verify all properties are correctly defined

### 2. Add Entity Configuration
- [ ] Copy `UserEmployerConfiguration.cs` to `PayrollSystem.Infrastructure/Data/Configurations/`
- [ ] Ensure configuration is picked up by `ApplyConfigurationsFromAssembly()`

### 3. Update DbContext
- [ ] Add `DbSet<ApplicationUser> ApplicationUsers` to `ApplicationDbContext`
- [ ] Add `DbSet<UserEmployer> UserEmployers` to `ApplicationDbContext`
- [ ] Configure ApplicationUser relationships in `OnModelCreating`:
  ```csharp
  modelBuilder.Entity<ApplicationUser>()
      .HasOne(u => u.PrimaryEmployer)
      .WithMany()
      .HasForeignKey(u => u.PrimaryEmployerId)
      .OnDelete(DeleteBehavior.SetNull);
  ```

---

## Phase 1B: Database Migration 🗄️

### 4. Create and Apply Migration
```bash
# Create migration
dotnet ef migrations add "AddAuthenticationEntities" \
    --project PayrollSystem.Infrastructure \
    --startup-project PayrollSystem.Web

# Review the migration file to ensure it looks correct

# Apply migration
dotnet ef database update \
    --project PayrollSystem.Infrastructure \
    --startup-project PayrollSystem.Web
```

- [ ] Migration created successfully
- [ ] Migration reviewed (check Up/Down methods)
- [ ] Database updated successfully
- [ ] Verify tables exist in SQL Server:
  - AspNetUsers (should have new columns)
  - UserEmployers (new table)

---

## Phase 1C: Services & Middleware 🔧

### 5. Add AccountService
- [ ] Copy `AccountService.cs` to `PayrollSystem.Infrastructure/Services/`
- [ ] Verify all dependencies are available (SignInManager, UserManager, etc.)
- [ ] Register service in `Program.cs`:
  ```csharp
  builder.Services.AddScoped<IAccountService, AccountService>();
  ```

### 6. Add TenantMiddleware
- [ ] Create `Middleware` folder in `PayrollSystem.Web`
- [ ] Copy `TenantMiddleware.cs` to `PayrollSystem.Web/Middleware/`
- [ ] Register middleware in `Program.cs` (see instructions file)

### 7. Update Program.cs
Follow the instructions in `Program.cs.instructions.txt`:
- [ ] Add required usings
- [ ] Replace Identity configuration with multi-tenant version
- [ ] Configure authentication cookies
- [ ] Register AccountService
- [ ] Add authentication state provider
- [ ] Add UseTenantMiddleware() to pipeline (AFTER UseAuthentication/UseAuthorization)
- [ ] Optional: Add authorization policies

---

## Phase 1D: Blazor UI Components 🎨

### 8. Create Layouts
- [ ] Create `Layouts` folder in `PayrollSystem.Web` (if not exists)
- [ ] Copy `EmptyLayout.razor` to `PayrollSystem.Web/Layouts/`
- [ ] Update `MainLayout.razor` using the example provided

### 9. Create Authentication Pages
- [ ] Create `Pages/Account` folder in `PayrollSystem.Web`
- [ ] Copy `Login.razor` to `PayrollSystem.Web/Pages/Account/`
- [ ] Copy `Logout.razor` to `PayrollSystem.Web/Pages/Account/`
- [ ] Verify MudBlazor components are available

### 10. Update App.razor (if needed)
Add authentication cascading parameter:
```razor
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly">
        @* ... existing router config ... *@
    </Router>
</CascadingAuthenticationState>
```

---

## Phase 1E: Data Seeding 🌱

### 11. Create Demo User
Update `DataSeeder.cs`:
- [ ] Add `SeedDemoUserAsync` method (see DATABASE_MIGRATION_GUIDE.md)
- [ ] Update `Program.cs` to call the new seed method
- [ ] Run application to seed demo user

### 12. Verify Demo Data
- [ ] Check database for demo user: `admin@democompany.com`
- [ ] Verify UserEmployer relationship exists
- [ ] Test credentials work

---

## Phase 1F: Testing & Verification ✅

### 13. Test Authentication Flow
- [ ] Navigate to `/login`
- [ ] Login with demo credentials
- [ ] Verify employer selection appears (if multiple employers)
- [ ] Verify redirect to home page after login
- [ ] Check browser dev tools - verify claims are set
- [ ] Verify EmployerId claim is present

### 14. Test Tenant Isolation
- [ ] Add logging to TenantMiddleware to verify it runs
- [ ] Check that TenantService.GetEmployerId() returns correct value
- [ ] Test database queries respect tenant filter
- [ ] Verify employee queries only show current employer's data

### 15. Test Logout
- [ ] Click logout from menu
- [ ] Verify redirect to login page
- [ ] Verify cannot access protected pages
- [ ] Verify session is cleared

### 16. Test Edge Cases
- [ ] Login with invalid credentials - should show error
- [ ] Login with inactive user - should show error
- [ ] Login with user who has no employers - should show error
- [ ] Test account lockout after multiple failed attempts
- [ ] Test accessing protected pages when not logged in

---

## Phase 1G: Security Hardening 🔒

### 17. Review Security Settings
- [ ] Verify RequireAuthentication is set on routes
- [ ] Check password requirements are appropriate
- [ ] Verify cookie settings (secure, httpOnly, etc.)
- [ ] Review lockout settings
- [ ] Check email confirmation requirement (disable for dev, enable for prod)

### 18. Add Authorization Attributes
Add to pages that require authentication:
```razor
@attribute [Authorize]
```

For admin-only pages:
```razor
@attribute [Authorize(Policy = "SystemAdminOnly")]
```

- [ ] Add [Authorize] to employee pages
- [ ] Add [Authorize] to department pages
- [ ] Add [Authorize] to payroll pages
- [ ] Keep [AllowAnonymous] on Login page

---

## Phase 1H: Documentation & Cleanup 📝

### 19. Update Documentation
- [ ] Update README.md with login instructions
- [ ] Document demo user credentials
- [ ] Add troubleshooting section
- [ ] Document authorization policies

### 20. Code Review
- [ ] Review all new code for consistency
- [ ] Check for TODO comments
- [ ] Verify error handling is adequate
- [ ] Check logging is appropriate
- [ ] Remove any debug code

---

## Common Issues & Solutions

### Issue: "Table already exists" migration error
**Solution**: Drop and recreate database, or manually remove conflicting tables

### Issue: Login redirects to login page in loop
**Solution**: 
- Check authentication middleware order in Program.cs
- Verify [AllowAnonymous] on Login.razor
- Check cookie configuration

### Issue: EmployerId claim not found
**Solution**:
- Verify AccountService sets the claim
- Check TenantMiddleware runs after authentication
- Inspect claims in browser dev tools (Application > Cookies)

### Issue: TenantService.GetEmployerId() returns null
**Solution**:
- Verify TenantMiddleware runs
- Check claim is named exactly "EmployerId"
- Add logging to middleware

### Issue: Can't see any employees after login
**Solution**:
- Verify demo data includes employees for demo employer
- Check EmployerId on Employee records matches user's employer
- Verify global query filter is working

---

## Next Steps (Phase 2)

After completing Phase 1, you'll be ready for:
- [ ] Employee CRUD pages
- [ ] Department management
- [ ] Advanced authorization (role-based access)
- [ ] User invitation workflow
- [ ] Profile management

---

## Verification Commands

```bash
# Check migration status
dotnet ef migrations list --project PayrollSystem.Infrastructure --startup-project PayrollSystem.Web

# View applied migrations in database
dotnet ef migrations has-pending-model-changes --project PayrollSystem.Infrastructure --startup-project PayrollSystem.Web

# Build solution
dotnet build

# Run application
dotnet run --project PayrollSystem.Web

# Run tests (after you create them)
dotnet test
```

---

## Success Criteria

✅ Phase 1 is complete when:
1. Users can log in with email/password
2. Users can select from available employers
3. Tenant context is automatically set on login
4. Protected pages require authentication
5. Users can only see data for their employer
6. Users can log out successfully
7. All tests pass
8. No security vulnerabilities identified

---

**Estimated Time**: 3-4 hours
**Created**: February 2026
**Sprint**: Sprint 2 - Phase 1
