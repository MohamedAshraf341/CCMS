using System;

namespace CCMS.BE.Data.Models
{
    public class AppSetting
    {
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? ValueType { get; set; }
    }
}
