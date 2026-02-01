using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Core.Entities.System;
using PayrollSystem.Core.Entities.Employers;
using PayrollSystem.Core.Entities.Employees;
using PayrollSystem.Core.Entities.Payroll;
using PayrollSystem.Core.Entities.Leaves;

namespace PayrollSystem.Infrastructure.Data.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await context.Database.MigrateAsync();

        await SeedRolesAsync(roleManager);
        await SeedSystemAdminAsync(userManager);
        await SeedDemoEmployerAsync(context, userManager);
        await SeedSystemSettingsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "SystemAdmin", "Admin", "HR", "Manager", "Employee" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedSystemAdminAsync(UserManager<User> userManager)
    {
        var adminEmail = "admin@payrollsystem.com";
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SystemAdmin");
            }
        }
    }

    private static async Task SeedDemoEmployerAsync(
        ApplicationDbContext context,
        UserManager<User> userManager)
    {
        if (await context.Employers.AnyAsync(e => e.CompanyName == "Demo Company Inc."))
        {
            return;
        }

        // Create Demo Employer
        var demoEmployer = new Employer
        {
            CompanyName = "Demo Company Inc.",
            TaxIdentificationNumber = "12-3456789",
            Address = "123 Business St",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            Country = "USA",
            Phone = "+1 (555) 123-4567",
            Email = "info@democompany.com",
            Website = "https://democompany.com",
            SubscriptionPlan = "Premium",
            SubscriptionStatus = "Active",
            SubscriptionStartDate = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system"
        };

        context.Employers.Add(demoEmployer);
        await context.SaveChangesAsync();

        // Create Demo Company Admin User
        var companyAdminEmail = "admin@democompany.com";
        var companyAdminUser = await userManager.FindByEmailAsync(companyAdminEmail);
        
        if (companyAdminUser == null)
        {
            companyAdminUser = new User
            {
                UserName = companyAdminEmail,
                Email = companyAdminEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            await userManager.CreateAsync(companyAdminUser, "DemoAdmin@123!");
            await userManager.AddToRoleAsync(companyAdminUser, "Admin");
        }

        // Link Admin User to Employer
        var employerUser = new EmployerUser
        {
            UserId = companyAdminUser.Id,
            EmployerId = demoEmployer.Id,
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system"
        };

        context.EmployerUsers.Add(employerUser);

        // Create Employer Settings
        var settings = new EmployerSettings
        {
            EmployerId = demoEmployer.Id,
            DefaultPayPeriodType = "BiWeekly",
            PayrollStartDay = "Monday",
            DefaultCurrency = "USD",
            TimeZone = "America/New_York",
            DateFormat = "MM/dd/yyyy",
            TimeFormat = "hh:mm tt",
            FiscalYearStartMonth = 1,
            AllowOvertime = true,
            OvertimeMultiplier = 1.5m,
            EnableTimesheets = true,
            RequireTimeApproval = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "system"
        };

        context.EmployerSettings.Add(settings);

        // Create Departments
        var departments = new[]
        {
            new Department
            {
                EmployerId = demoEmployer.Id,
                Name = "Engineering",
                Description = "Software development and IT",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            },
            new Department
            {
                EmployerId = demoEmployer.Id,
                Name = "Human Resources",
                Description = "HR and employee relations",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            },
            new Department
            {
                EmployerId = demoEmployer.Id,
                Name = "Finance",
                Description = "Accounting and financial operations",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }
        };

        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        // Create Sample Employees
        var employees = new[]
        {
            new Employee
            {
                EmployerId = demoEmployer.Id,
                EmployeeNumber = "EMP001",
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@democompany.com",
                Phone = "+1 (555) 111-1111",
                DepartmentId = departments[0].Id,
                JobTitle = "Senior Software Engineer",
                EmploymentType = "FullTime",
                EmploymentStatus = "Active",
                HireDate = DateTime.UtcNow.AddYears(-2),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            },
            new Employee
            {
                EmployerId = demoEmployer.Id,
                EmployeeNumber = "EMP002",
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@democompany.com",
                Phone = "+1 (555) 222-2222",
                DepartmentId = departments[1].Id,
                JobTitle = "HR Manager",
                EmploymentType = "FullTime",
                EmploymentStatus = "Active",
                HireDate = DateTime.UtcNow.AddYears(-3),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }
        };

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        // Create Employee Compensations
        var compensations = new[]
        {
            new EmployeeCompensation
            {
                EmployerId = demoEmployer.Id,
                EmployeeId = employees[0].Id,
                CompensationType = "Salary",
                Amount = 95000m,
                Currency = "USD",
                PayFrequency = "BiWeekly",
                EffectiveDate = employees[0].HireDate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            },
            new EmployeeCompensation
            {
                EmployerId = demoEmployer.Id,
                EmployeeId = employees[1].Id,
                CompensationType = "Salary",
                Amount = 75000m,
                Currency = "USD",
                PayFrequency = "BiWeekly",
                EffectiveDate = employees[1].HireDate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }
        };

        context.EmployeeCompensations.AddRange(compensations);

        // Create Deduction Types
        var deductionTypes = new[]
        {
            new DeductionType
            {
                EmployerId = demoEmployer.Id,
                Name = "Health Insurance",
                Description = "Employee health insurance premium",
                DeductionCategory = "PreTax",
                IsPercentage = false,
                DefaultAmount = 150m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }
        };

        context.DeductionTypes.AddRange(deductionTypes);

        // Create Leave Types
        var leaveTypes = new[]
        {
            new LeaveType
            {
                EmployerId = demoEmployer.Id,
                Name = "Vacation",
                Description = "Paid vacation time",
                IsPaid = true,
                DefaultDaysPerYear = 15,
                AccrualRate = 1.25m,
                MaxCarryOver = 40,
                RequiresApproval = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }
        };

        context.LeaveTypes.AddRange(leaveTypes);

        await context.SaveChangesAsync();
    }

    private static async Task SeedSystemSettingsAsync(ApplicationDbContext context)
    {
        if (await context.SystemSettings.AnyAsync())
        {
            return;
        }

        var settings = new[]
        {
            new SystemSettings
            {
                SettingKey = "MaintenanceMode",
                SettingValue = "false",
                Description = "Enable/disable system maintenance mode",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }
        };

        context.SystemSettings.AddRange(settings);
        await context.SaveChangesAsync();
    }
}