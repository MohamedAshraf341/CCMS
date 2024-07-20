using System;
using System.Collections.Generic;
namespace CCMS.Common.Dto
{
    public class BranchDto
    {
        public Guid Id { get; set; }
        public string Restaurant { get; set; }
        public string Address { get; set; }
        public List<PhoneNumberDto>? phoneNumbers { get; set; }
    }
}
