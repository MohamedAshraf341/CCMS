using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto
{
    public class UsersDto
    {
        public string UserId { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string Restaurant { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string? Gender { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string phone { get; set; }
        public List<string> Roles { get; set; }
    }
}
