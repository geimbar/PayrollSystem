using PayrollSystem.Core.Entities.Base;

namespace PayrollSystem.Core.Entities.System;

public class SystemSettings : BaseEntity
{
    public int Id { get; set; }
    public string SettingKey { get; set; }
    public string SettingValue { get; set; }
    public string Description { get; set; }
}