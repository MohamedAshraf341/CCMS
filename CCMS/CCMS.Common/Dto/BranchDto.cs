using System;
using System.Collections.Generic;
namespace CCMS.Common.Dto
{
    public class BranchDto
    {
        public Guid Id { get; set; }
        public string? Reasturant { get; set; }
        public string? Government { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public List<string>? Moderators {  get; set; } 
        public List<PhoneNumberDto>? phoneNumbers { get; set; }
    }
}
