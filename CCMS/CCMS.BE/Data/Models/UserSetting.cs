using System;

namespace CCMS.BE.Data.Models
{
    public class UserSetting
    {
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? ValueType { get; set; }
        public string UserId { get; set; }
        public  ApplicationUser User {  get; set; } 
    }
}
