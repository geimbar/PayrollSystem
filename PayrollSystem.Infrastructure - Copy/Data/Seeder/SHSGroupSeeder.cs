using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayrollSystem.Core.Entities;
using PayrollSystem.Core.Entities.Core;
using PayrollSystem.Core.Entities.Employment;
using PayrollSystem.Core.Entities.Identity;
using PayrollSystem.Core.Entities.People;
using PayrollSystem.Infrastructure.Data;

namespace PayrollSystem.Infrastructure.Data.Seeder;

public class SHSGroupSeeder
{
    private readonly ApplicationDbContext _context;

    public SHSGroupSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Check if data already exists
        if (await _context.Tenants.AnyAsync())
        {
            Console.WriteLine("Database already seeded. Skipping...");
            return;
        }

        Console.WriteLine("Seeding SHS Group data...");

        // 1. Create Tenant: "SHS Group"
        var shsGroup = new Tenant
        {
            TenantName = "SHS Group",
            LegalName = "Shit-hot-Software Group Ltd.",
            TaxId = "SHS-TAX-001",
            Address = "123 Tech Street",
            City = "San Francisco",
            State = "CA",
            ZipCode = "94105",
            Country = "USA",
            Phone = "+1-415-555-0100",
            Email = "info@shsgroup.com",
            IsActive = true
        };
        _context.Tenants.Add(shsGroup);
        await _context.SaveChangesAsync();

        // 2. Create Companies under SHS Group
        var companies = new[]
        {
            new Company
            {
                TenantId = shsGroup.Id,
                CompanyName = "Global",
                LegalName = "SHS Global Inc.",
                TaxId = "SHS-GLB-001",
                Address = "123 Tech Street",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94105",
                Country = "USA",
                Email = "global@shsgroup.com",
                IsActive = true
            },
            new Company
            {
                TenantId = shsGroup.Id,
                CompanyName = "Europe",
                LegalName = "SHS Europe GmbH",
                TaxId = "DE-SHS-EUR-001",
                Address = "456 Innovation Ave",
                City = "Berlin",
                State = "Berlin",
                ZipCode = "10115",
                Country = "Germany",
                Email = "europe@shsgroup.com",
                IsActive = true
            },
            new Company
            {
                TenantId = shsGroup.Id,
                CompanyName = "Development",
                LegalName = "SHS Development Ltd.",
                TaxId = "UK-SHS-DEV-001",
                Address = "789 Code Lane",
                City = "London",
                State = "Greater London",
                ZipCode = "EC1A 1BB",
                Country = "United Kingdom",
                Email = "dev@shsgroup.com",
                IsActive = true
            }
        };
        _context.Companies.AddRange(companies);
        await _context.SaveChangesAsync();

        var global = companies[0];
        var europe = companies[1];
        var development = companies[2];

        // 3. Create Departments for each company
        var departments = new List<Department>
        {
            // Global departments
            new Department { CompanyId = global.Id, DepartmentName = "Executive", DepartmentCode = "GLB-EXEC", CostCenter = "CC-001" },
            new Department { CompanyId = global.Id, DepartmentName = "Finance", DepartmentCode = "GLB-FIN", CostCenter = "CC-002" },
            new Department { CompanyId = global.Id, DepartmentName = "HR", DepartmentCode = "GLB-HR", CostCenter = "CC-003" },
            
            // Europe departments
            new Department { CompanyId = europe.Id, DepartmentName = "Engineering", DepartmentCode = "EUR-ENG", CostCenter = "CC-101" },
            new Department { CompanyId = europe.Id, DepartmentName = "Sales", DepartmentCode = "EUR-SALES", CostCenter = "CC-102" },
            new Department { CompanyId = europe.Id, DepartmentName = "Marketing", DepartmentCode = "EUR-MKT", CostCenter = "CC-103" },
            
            // Development departments
            new Department { CompanyId = development.Id, DepartmentName = "R&D", DepartmentCode = "DEV-RND", CostCenter = "CC-201" },
            new Department { CompanyId = development.Id, DepartmentName = "QA", DepartmentCode = "DEV-QA", CostCenter = "CC-202" },
            new Department { CompanyId = development.Id, DepartmentName = "DevOps", DepartmentCode = "DEV-OPS", CostCenter = "CC-203" }
        };
        _context.Departments.AddRange(departments);
        await _context.SaveChangesAsync();

        // 4. Create Employees (Tenant level - these are people)
        var employees = new[]
        {
            // Executive/Board member
            new Employee
            {
                TenantId = shsGroup.Id,
                FirstName = "John",
                LastName = "CEO-Anderson",
                Email = "john.anderson@shsgroup.com",
                PersonalEmail = "john.a@personal.com",
                Phone = "+1-415-555-0101",
                DateOfBirth = new DateTime(1975, 3, 15),
                SSN = "123-45-6789",
                Address = "100 Executive Way",
                City = "San Francisco",
                ZipCode = "94102",
                Country = "USA",
                IsActive = true
            },
            
            // Engineering Manager (works in Europe + Board member in Global)
            new Employee
            {
                TenantId = shsGroup.Id,
                FirstName = "Sarah",
                LastName = "Schmidt",
                Email = "sarah.schmidt@shsgroup.com",
                PersonalEmail = "s.schmidt@personal.com",
                Phone = "+49-30-555-0201",
                DateOfBirth = new DateTime(1985, 7, 22),
                SSN = "DE-456-789-012",
                Address = "Unter den Linden 10",
                City = "Berlin",
                ZipCode = "10117",
                Country = "Germany",
                IsActive = true
            },
            
            // Developer (only in Development)
            new Employee
            {
                TenantId = shsGroup.Id,
                FirstName = "Michael",
                LastName = "Brown",
                Email = "michael.brown@shsgroup.com",
                PersonalEmail = "m.brown@personal.com",
                Phone = "+44-20-555-0301",
                DateOfBirth = new DateTime(1990, 11, 5),
                SSN = "GB-789-012-345",
                Address = "10 Downing Street",
                City = "London",
                ZipCode = "SW1A 2AA",
                Country = "United Kingdom",
                IsActive = true
            },
            
            // Engineer (works in Europe)
            new Employee
            {
                TenantId = shsGroup.Id,
                FirstName = "Emma",
                LastName = "Mueller",
                Email = "emma.mueller@shsgroup.com",
                PersonalEmail = "e.mueller@personal.com",
                Phone = "+49-30-555-0401",
                DateOfBirth = new DateTime(1992, 5, 18),
                SSN = "DE-567-890-123",
                Address = "Friedrichstraße 50",
                City = "Berlin",
                ZipCode = "10117",
                Country = "Germany",
                IsActive = true
            },
            
            // Sales (Europe)
            new Employee
            {
                TenantId = shsGroup.Id,
                FirstName = "Hans",
                LastName = "Weber",
                Email = "hans.weber@shsgroup.com",
                PersonalEmail = "h.weber@personal.com",
                Phone = "+49-30-555-0501",
                DateOfBirth = new DateTime(1988, 9, 30),
                SSN = "DE-678-901-234",
                Address = "Kurfürstendamm 100",
                City = "Berlin",
                ZipCode = "10711",
                Country = "Germany",
                IsActive = true
            }
        };
        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();

        var johnCEO = employees[0];
        var sarah = employees[1];
        var michael = employees[2];
        var emma = employees[3];
        var hans = employees[4];

        // 5. Create Employments (Jobs linking employees to companies)
        var employments = new List<Employment>
        {
            // John CEO-Anderson: CEO in Global (main job) + Board member advisory fees from other companies
            new Employment
            {
                EmployeeId = johnCEO.Id,
                CompanyId = global.Id,
                DepartmentId = departments.First(d => d.CompanyId == global.Id && d.DepartmentName == "Executive").Id,
                JobTitle = "Chief Executive Officer",
                EmploymentType = "Full-time",
                EmploymentStatus = "Active",
                StartDate = new DateTime(2015, 1, 1),
                HoursPerWeek = 40,
                BaseSalary = 250000,
                Currency = "USD",
                PaymentFrequency = "Monthly",
                IsPrimaryJob = true,
                TaxCountry = "USA",
                TaxState = "CA"
            },
            
            // Sarah Schmidt: Engineering Manager in Europe (main job)
            new Employment
            {
                EmployeeId = sarah.Id,
                CompanyId = europe.Id,
                DepartmentId = departments.First(d => d.CompanyId == europe.Id && d.DepartmentName == "Engineering").Id,
                JobTitle = "VP Engineering",
                EmploymentType = "Full-time",
                EmploymentStatus = "Active",
                StartDate = new DateTime(2018, 3, 1),
                HoursPerWeek = 40,
                BaseSalary = 120000,
                Currency = "EUR",
                PaymentFrequency = "Monthly",
                IsPrimaryJob = true,
                TaxCountry = "Germany",
                TaxState = "Berlin"
            },
            
            // Sarah Schmidt: Board Member in Global (secondary advisory role)
            new Employment
            {
                EmployeeId = sarah.Id,
                CompanyId = global.Id,
                DepartmentId = departments.First(d => d.CompanyId == global.Id && d.DepartmentName == "Executive").Id,
                JobTitle = "Board Member - Technical Advisor",
                EmploymentType = "Board",
                EmploymentStatus = "Active",
                StartDate = new DateTime(2020, 1, 1),
                HoursPerWeek = 5,
                BaseSalary = 25000,
                Currency = "USD",
                PaymentFrequency = "Quarterly",
                IsPrimaryJob = false,
                TaxCountry = "USA",
                TaxState = "CA"
            },
            
            // Michael Brown: Senior Developer in Development
            new Employment
            {
                EmployeeId = michael.Id,
                CompanyId = development.Id,
                DepartmentId = departments.First(d => d.CompanyId == development.Id && d.DepartmentName == "R&D").Id,
                JobTitle = "Senior Software Engineer",
                EmploymentType = "Full-time",
                EmploymentStatus = "Active",
                StartDate = new DateTime(2019, 6, 1),
                HoursPerWeek = 40,
                BaseSalary = 85000,
                Currency = "GBP",
                PaymentFrequency = "Monthly",
                IsPrimaryJob = true,
                TaxCountry = "United Kingdom",
                TaxState = "England"
            },
            
            // Emma Mueller: Software Engineer in Europe
            new Employment
            {
                EmployeeId = emma.Id,
                CompanyId = europe.Id,
                DepartmentId = departments.First(d => d.CompanyId == europe.Id && d.DepartmentName == "Engineering").Id,
                JobTitle = "Software Engineer",
                EmploymentType = "Full-time",
                EmploymentStatus = "Active",
                StartDate = new DateTime(2021, 9, 1),
                HoursPerWeek = 40,
                BaseSalary = 70000,
                Currency = "EUR",
                PaymentFrequency = "Monthly",
                IsPrimaryJob = true,
                TaxCountry = "Germany",
                TaxState = "Berlin"
            },
            
            // Hans Weber: Sales Manager in Europe
            new Employment
            {
                EmployeeId = hans.Id,
                CompanyId = europe.Id,
                DepartmentId = departments.First(d => d.CompanyId == europe.Id && d.DepartmentName == "Sales").Id,
                JobTitle = "Sales Manager",
                EmploymentType = "Full-time",
                EmploymentStatus = "Active",
                StartDate = new DateTime(2017, 4, 1),
                HoursPerWeek = 40,
                BaseSalary = 75000,
                Currency = "EUR",
                PaymentFrequency = "Monthly",
                IsPrimaryJob = true,
                TaxCountry = "Germany",
                TaxState = "Berlin"
            }
        };
        _context.Employments.AddRange(employments);
        await _context.SaveChangesAsync();

        Console.WriteLine("SHS Group data seeded successfully!");
        Console.WriteLine($"- Tenant: {shsGroup.TenantName}");
        Console.WriteLine($"- Companies: {companies.Length}");
        Console.WriteLine($"- Departments: {departments.Count}");
        Console.WriteLine($"- Employees: {employees.Length}");
        Console.WriteLine($"- Employments (Jobs): {employments.Count}");
        Console.WriteLine("");
        Console.WriteLine("Key Statistics:");
        Console.WriteLine($"- Total employees in SHS Group: {employees.Length}");
        Console.WriteLine($"- Employees with multiple jobs: 1 (Sarah Schmidt)");
        Console.WriteLine($"- Global company jobs: {employments.Count(e => e.CompanyId == global.Id)}");
        Console.WriteLine($"- Europe company jobs: {employments.Count(e => e.CompanyId == europe.Id)}");
        Console.WriteLine($"- Development company jobs: {employments.Count(e => e.CompanyId == development.Id)}");
    }

    public async Task SeedDemoUserAsync(UserManager<ApplicationUser> userManager)
    {
        // Check if demo user exists
        var demoUser = await userManager.FindByEmailAsync("admin@shsgroup.com");

        if (demoUser != null)
        {
            Console.WriteLine("Demo user already exists. Skipping user creation...");
            return;
        }

        Console.WriteLine("Creating demo users...");

        var shsGroup = await _context.Tenants.FirstAsync();
        var global = await _context.Companies.FirstAsync(c => c.CompanyName == "Global");
        var europe = await _context.Companies.FirstAsync(c => c.CompanyName == "Europe");

        // 1. System Admin (full access to everything)
        var systemAdmin = new ApplicationUser
        {
            UserName = "admin@shsgroup.com",
            Email = "admin@shsgroup.com",
            EmailConfirmed = true,
            FullName = "System Administrator",
            IsSystemAdmin = true,
            PrimaryTenantId = shsGroup.Id,
            IsActive = true
        };
        await userManager.CreateAsync(systemAdmin, "Admin123!");

        _context.UserTenants.Add(new UserTenant
        {
            UserId = systemAdmin.Id,
            TenantId = shsGroup.Id,
            Role = "SystemAdmin"
        });

        // 2. Tenant Admin (access to all companies in SHS Group)
        var tenantAdmin = new ApplicationUser
        {
            UserName = "tenant.admin@shsgroup.com",
            Email = "tenant.admin@shsgroup.com",
            EmailConfirmed = true,
            FullName = "Tenant Admin",
            IsSystemAdmin = false,
            PrimaryTenantId = shsGroup.Id,
            IsActive = true
        };
        await userManager.CreateAsync(tenantAdmin, "TenantAdmin123!");

        _context.UserTenants.Add(new UserTenant
        {
            UserId = tenantAdmin.Id,
            TenantId = shsGroup.Id,
            CompanyId = null, // Access to all companies
            Role = "TenantAdmin"
        });

        // 3. Company Admin (access only to Europe company)
        var companyAdmin = new ApplicationUser
        {
            UserName = "europe.admin@shsgroup.com",
            Email = "europe.admin@shsgroup.com",
            EmailConfirmed = true,
            FullName = "Europe Admin",
            IsSystemAdmin = false,
            PrimaryTenantId = shsGroup.Id,
            IsActive = true
        };
        await userManager.CreateAsync(companyAdmin, "EuropeAdmin123!");

        _context.UserTenants.Add(new UserTenant
        {
            UserId = companyAdmin.Id,
            TenantId = shsGroup.Id,
            CompanyId = europe.Id, // Only Europe company
            Role = "CompanyAdmin"
        });

        await _context.SaveChangesAsync();

        Console.WriteLine("");
        Console.WriteLine("Demo users created:");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. System Admin:");
        Console.WriteLine("   Email: admin@shsgroup.com");
        Console.WriteLine("   Password: Admin123!");
        Console.WriteLine("   Access: All tenants, all companies");
        Console.WriteLine("");
        Console.WriteLine("2. Tenant Admin:");
        Console.WriteLine("   Email: tenant.admin@shsgroup.com");
        Console.WriteLine("   Password: TenantAdmin123!");
        Console.WriteLine("   Access: SHS Group (all 3 companies)");
        Console.WriteLine("");
        Console.WriteLine("3. Company Admin:");
        Console.WriteLine("   Email: europe.admin@shsgroup.com");
        Console.WriteLine("   Password: EuropeAdmin123!");
        Console.WriteLine("   Access: Europe company only");
        Console.WriteLine("----------------------------------------");
    }
}
