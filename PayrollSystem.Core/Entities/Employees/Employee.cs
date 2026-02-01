using PayrollSystem.Core.Entities.Base;
using PayrollSystem.Core.Entities.System;
using PayrollSystem.Core.Entities.Payroll;
using PayrollSystem.Core.Entities.Benefits;
using PayrollSystem.Core.Entities.Leaves;
using PayrollSystem.Core.Entities.TimeTracking;

namespace PayrollSystem.Core.Entities.Employees;

public class Employee : TenantEntity
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string EmployeeNumber { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    // Personal information
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string SSN { get; set; }

    // Address
    public string Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }

    // Employment details
    public int DepartmentId { get; set; }
    public string JobTitle { get; set; }
    public string EmploymentType { get; set; }
    public string EmploymentStatus { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string? TerminationReason { get; set; }
    public bool IsActive { get; set; }

    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<EmployeeCompensation> Compensations { get; set; }
    public virtual ICollection<EmployeeBankAccount> BankAccounts { get; set; }
    public virtual ICollection<TimeSheet> TimeSheets { get; set; }
    public virtual ICollection<PayrollTransaction> PayrollTransactions { get; set; }
    public virtual ICollection<EmployeeDeduction> Deductions { get; set; }
    public virtual ICollection<EmployeeBenefit> Benefits { get; set; }
    public virtual ICollection<EmployeeLeaveBalance> LeaveBalances { get; set; }
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
}