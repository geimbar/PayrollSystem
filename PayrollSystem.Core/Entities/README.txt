# PayrollSystem.Core.Entities - Clean Structure

## Folder Structure

```
Entities/
├── Core/                    # Core tenant/company entities
│   ├── Tenant.cs
│   ├── Company.cs
│   └── UserTenant.cs
│
├── Identity/                # Authentication
│   └── ApplicationUser.cs
│
├── People/                  # Person information
│   ├── Employee.cs
│   ├── NextOfKin.cs
│   └── BankAccount.cs
│
├── Employment/              # Jobs and positions
│   ├── Employment.cs
│   ├── Department.cs
│   ├── Compensation.cs
│   └── LeaveBalance.cs
│
├── Tax/                     # Tax-related entities
│   └── TaxCard.cs
│
└── (Keep your existing folders if they have working code):
    ├── Benefits/
    ├── Payroll/
    ├── Leaves/
    ├── System/
    ├── Timetracking/
    └── Base/
```

## Installation Instructions

1. **Backup your current Entities folder** (just in case)
2. **Delete PayrollSystem.Core/Entities folder**
3. **Extract this zip** to PayrollSystem.Core/
4. **Copy back any working code** from your backup for:
   - Benefits/
   - Payroll/
   - Leaves/
   - System/
   - Timetracking/
   - Base/

## What's Included

### NEW Entities (Complete):
- ✅ Core/Tenant.cs
- ✅ Core/Company.cs
- ✅ Core/UserTenant.cs
- ✅ Identity/ApplicationUser.cs
- ✅ People/Employee.cs
- ✅ People/NextOfKin.cs
- ✅ People/BankAccount.cs
- ✅ Employment/Employment.cs
- ✅ Employment/Department.cs
- ✅ Employment/Compensation.cs
- ✅ Employment/LeaveBalance.cs
- ✅ Tax/TaxCard.cs

### NOT Included (Copy from your existing code):
- Benefits/ folder (keep your existing)
- Payroll/ folder (keep your existing)
- Leaves/ folder (keep your existing)
- System/ folder (keep your existing)
- Timetracking/ folder (keep your existing)
- Base/ folder (keep your existing - BUT update TenantEntity if it exists)

## After Extraction

1. Copy your existing folders back (Benefits, Payroll, Leaves, System, Timetracking, Base)
2. If you have Base/TenantEntity.cs, update it to use TenantId instead of EmployerId
3. Update ApplicationDbContext.cs (see separate guide)
4. Update TenantService.cs (see separate guide)
5. Run migrations

## Key Changes

- Employer → Tenant (top level org)
- Added Company (financial entities)
- Added Employment (job/position linking employee to company)
- Employee now at Tenant level (not Company level)
- All entities use proper namespaces (PayrollSystem.Core.Entities.xxx)
