# ============================================================================
# Entity Reorganization Script
# Sprint 2 - Phase 1.5
# ============================================================================

$ErrorActionPreference = "Stop"
$entitiesPath = "PayrollSystem.Core\Entities"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Entity Reorganization - Sprint 2 Phase 1.5" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Create new folder structure
Write-Host "Step 1: Creating new folder structure..." -ForegroundColor Yellow
$newFolders = @(
    "$entitiesPath\Core",
    "$entitiesPath\Identity",
    "$entitiesPath\People",
    "$entitiesPath\Employment",
    "$entitiesPath\Tax"
)

foreach ($folder in $newFolders) {
    if (-not (Test-Path $folder)) {
        New-Item -Path $folder -ItemType Directory -Force | Out-Null
        Write-Host "  Created: $folder" -ForegroundColor Green
    }
}
Write-Host ""

# Step 2: Move existing files
Write-Host "Step 2: Moving existing files..." -ForegroundColor Yellow

# Department: Employees to Employment
if (Test-Path "$entitiesPath\Employees\Department.cs") {
    Move-Item -Path "$entitiesPath\Employees\Department.cs" -Destination "$entitiesPath\Employment\Department.cs" -Force
    Write-Host "  Moved: Department.cs to Employment/" -ForegroundColor Green
}

# TaxBracket: Audit to Tax
if (Test-Path "$entitiesPath\Audit\TaxBracket.cs") {
    Move-Item -Path "$entitiesPath\Audit\TaxBracket.cs" -Destination "$entitiesPath\Tax\TaxBracket.cs" -Force
    Write-Host "  Moved: TaxBracket.cs to Tax/" -ForegroundColor Green
}

# PayrollTax: Payroll to Tax
if (Test-Path "$entitiesPath\Payroll\PayrollTax.cs") {
    Move-Item -Path "$entitiesPath\Payroll\PayrollTax.cs" -Destination "$entitiesPath\Tax\PayrollTax.cs" -Force
    Write-Host "  Moved: PayrollTax.cs to Tax/" -ForegroundColor Green
}

# AuditLog: Audit to System
if (Test-Path "$entitiesPath\Audit\AuditLog.cs") {
    Move-Item -Path "$entitiesPath\Audit\AuditLog.cs" -Destination "$entitiesPath\System\AuditLog.cs" -Force
    Write-Host "  Moved: AuditLog.cs to System/" -ForegroundColor Green
}
Write-Host ""

# Step 3: Delete old files
Write-Host "Step 3: Deleting obsolete files..." -ForegroundColor Yellow

$filesToDelete = @(
    "$entitiesPath\Employers\Employer.cs",
    "$entitiesPath\Employers\EmployerSettings.cs",
    "$entitiesPath\Employers\EmployerUser.cs",
    "$entitiesPath\Employees\EmployeeBankAccount.cs",
    "$entitiesPath\Employees\EmployeeCompensation.cs",
    "$entitiesPath\Leaves\EmployeeLeaveBalance.cs",
    "$entitiesPath\System\User.cs"
)

foreach ($file in $filesToDelete) {
    if (Test-Path $file) {
        Remove-Item -Path $file -Force
        $fileName = Split-Path $file -Leaf
        Write-Host "  Deleted: $fileName" -ForegroundColor Red
    }
}
Write-Host ""

# Step 4: Delete empty folders
Write-Host "Step 4: Cleaning up empty folders..." -ForegroundColor Yellow

$foldersToDelete = @(
    "$entitiesPath\Employers",
    "$entitiesPath\Audit"
)

foreach ($folder in $foldersToDelete) {
    if (Test-Path $folder) {
        $items = Get-ChildItem -Path $folder -Recurse
        if ($items.Count -eq 0) {
            Remove-Item -Path $folder -Recurse -Force
            $folderName = Split-Path $folder -Leaf
            Write-Host "  Deleted empty folder: $folderName" -ForegroundColor Red
        }
    }
}
Write-Host ""

# Step 5: Create placeholder files for new entities
Write-Host "Step 5: Creating placeholder files for new entities..." -ForegroundColor Yellow

$newFiles = @(
    "$entitiesPath\Core\Tenant.cs",
    "$entitiesPath\Core\Company.cs",
    "$entitiesPath\Core\UserTenant.cs",
    "$entitiesPath\Identity\ApplicationUser.cs",
    "$entitiesPath\People\Employee.cs",
    "$entitiesPath\People\NextOfKin.cs",
    "$entitiesPath\People\BankAccount.cs",
    "$entitiesPath\Employment\Employment.cs",
    "$entitiesPath\Employment\Compensation.cs",
    "$entitiesPath\Employment\LeaveBalance.cs",
    "$entitiesPath\Tax\TaxCard.cs"
)

foreach ($file in $newFiles) {
    if (-not (Test-Path $file)) {
        New-Item -Path $file -ItemType File -Force | Out-Null
        $fileName = Split-Path $file -Leaf
        Write-Host "  Created: $fileName" -ForegroundColor Green
    }
}
Write-Host ""

# Step 6: Summary
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Reorganization Complete!" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Copy entity code from ALL_FILES_PART1.txt to the new files" -ForegroundColor White
Write-Host "2. Update namespace in moved files:" -ForegroundColor White
Write-Host "   - Employment/Department.cs" -ForegroundColor Gray
Write-Host "   - Tax/TaxBracket.cs" -ForegroundColor Gray
Write-Host "   - Tax/PayrollTax.cs" -ForegroundColor Gray
Write-Host "   - System/AuditLog.cs" -ForegroundColor Gray
Write-Host "3. Update references in existing files" -ForegroundColor White
Write-Host "4. Update Base/TenantEntity.cs to use TenantId" -ForegroundColor White
Write-Host "5. Run the database migration script" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to continue..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
