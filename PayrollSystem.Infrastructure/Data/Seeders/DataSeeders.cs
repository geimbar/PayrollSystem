using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Core.Entities;
using PayrollSystem.Core.Entities.Employees;
using PayrollSystem.Core.Entities.Employers;
using PayrollSystem.Infrastructure.Data;

namespace PayrollSystem.Infrastructure.Data.Seeder;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Check if data already exists
        if (await _context.Employers.AnyAsync())
        {
            Console.WriteLine("Database already seeded. Skipping...");
            return;
        }

        Console.WriteLine("Seeding database...");

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
            Phone = "(555) 123-4567",
            Email = "contact@democompany.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        _context.Employers.Add(demoEmployer);
        await _context.SaveChangesAsync();

        // Create Departments
        var departments = new[]
        {
            new Department
            {
                EmployerId = demoEmployer.Id,
                Name = "Engineering",
                Description = "Software development team",
                CreatedAt = DateTime.UtcNow
            },
            new Department
            {
                EmployerId = demoEmployer.Id,
                Name = "HR",
                Description = "Human Resources",
                CreatedAt = DateTime.UtcNow
            },
            new Department
            {
                EmployerId = demoEmployer.Id,
                Name = "Finance",
                Description = "Finance and Accounting",
                CreatedAt = DateTime.UtcNow
            }
        };
        _context.Departments.AddRange(departments);
        await _context.SaveChangesAsync();

        // Create Sample Employees
        var employees = new[]
        {
            new Employee
            {
                EmployerId = demoEmployer.Id,
                DepartmentId = departments[0].Id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@democompany.com",
                Phone = "(555) 111-2222",
                DateOfBirth = new DateTime(1985, 5, 15),
                HireDate = new DateTime(2020, 1, 15),
                EmploymentStatus = "Active",
                JobTitle = "Senior Software Engineer",
                Address = "456 Tech Ave",
                City = "New York",
                State = "NY",
                ZipCode = "10002",
                Country = "USA",
                CreatedAt = DateTime.UtcNow
            },
            new Employee
            {
                EmployerId = demoEmployer.Id,
                DepartmentId = departments[1].Id,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@democompany.com",
                Phone = "(555) 333-4444",
                DateOfBirth = new DateTime(1990, 8, 22),
                HireDate = new DateTime(2021, 3, 1),
                EmploymentStatus = "Active",
                JobTitle = "HR Manager",
                Address = "789 Office Blvd",
                City = "New York",
                State = "NY",
                ZipCode = "10003",
                Country = "USA",
                CreatedAt = DateTime.UtcNow
            }
        };
        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();

        Console.WriteLine("Database seeded successfully!");
        Console.WriteLine($"Created employer: {demoEmployer.CompanyName}");
        Console.WriteLine($"Created {departments.Length} departments");
        Console.WriteLine($"Created {employees.Length} employees");
    }

    /// <summary>
    /// Seeds a demo user for testing authentication
    /// Call this AFTER SeedAsync() to ensure employer exists
    /// </summary>
    public async Task SeedDemoUserAsync(UserManager<ApplicationUser> userManager)
    {
        // Check if demo user exists
        var demoUser = await userManager.FindByEmailAsync("admin@democompany.com");

        if (demoUser != null)
        {
            Console.WriteLine("Demo user already exists. Skipping user creation...");
            return;
        }

        Console.WriteLine("Creating demo user...");

        // Get the demo employer
        var demoEmployer = await _context.Employers
            .FirstOrDefaultAsync(e => e.CompanyName == "Demo Company Inc.");

        if (demoEmployer == null)
        {
            Console.WriteLine("ERROR: Demo employer not found. Run SeedAsync() first.");
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

            Console.WriteLine("Demo user created successfully!");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Login Credentials:");
            Console.WriteLine("  Email: admin@democompany.com");
            Console.WriteLine("  Password: Admin123!");
            Console.WriteLine("  Company: Demo Company Inc.");
            Console.WriteLine("----------------------------------------");
        }
        else
        {
            Console.WriteLine("ERROR: Failed to create demo user:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.Description}");
            }
        }
    }

    /// <summary>
    /// Optional: Create additional test users
    /// </summary>
    public async Task SeedAdditionalUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var demoEmployer = await _context.Employers
            .FirstOrDefaultAsync(e => e.CompanyName == "Demo Company Inc.");

        if (demoEmployer == null) return;

        // Create a regular user (non-admin)
        var regularUser = await userManager.FindByEmailAsync("user@democompany.com");
        if (regularUser == null)
        {
            regularUser = new ApplicationUser
            {
                UserName = "user@democompany.com",
                Email = "user@democompany.com",
                EmailConfirmed = true,
                FullName = "Regular User",
                PrimaryEmployerId = demoEmployer.Id,
                IsSystemAdmin = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(regularUser, "User123!");
            if (result.Succeeded)
            {
                _context.UserEmployers.Add(new UserEmployer
                {
                    UserId = regularUser.Id,
                    EmployerId = demoEmployer.Id,
                    Role = "User",
                    GrantedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                Console.WriteLine("Regular user created: user@democompany.com / User123!");
            }
        }

        // Create a manager user
        var managerUser = await userManager.FindByEmailAsync("manager@democompany.com");
        if (managerUser == null)
        {
            managerUser = new ApplicationUser
            {
                UserName = "manager@democompany.com",
                Email = "manager@democompany.com",
                EmailConfirmed = true,
                FullName = "Department Manager",
                PrimaryEmployerId = demoEmployer.Id,
                IsSystemAdmin = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(managerUser, "Manager123!");
            if (result.Succeeded)
            {
                _context.UserEmployers.Add(new UserEmployer
                {
                    UserId = managerUser.Id,
                    EmployerId = demoEmployer.Id,
                    Role = "Manager",
                    GrantedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                Console.WriteLine("Manager user created: manager@democompany.com / Manager123!");
            }
        }
    }
}
