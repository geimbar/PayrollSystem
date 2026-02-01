using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.Employers;

public class EmployerSettings : TenantEntity
{
    public int Id { get; set; }

    // Payroll settings
    public string DefaultPayPeriodType { get; set; }
    public string PayrollStartDay { get; set; }
    public string DefaultCurrency { get; set; }

    // Regional settings
    public string TimeZone { get; set; }
    public string DateFormat { get; set; }
    public string TimeFormat { get; set; }
    public int FiscalYearStartMonth { get; set; }

    // Overtime settings
    public bool AllowOvertime { get; set; }
    public decimal OvertimeMultiplier { get; set; }

    // Timesheet settings
    public bool EnableTimesheets { get; set; }
    public bool RequireTimeApproval { get; set; }
}