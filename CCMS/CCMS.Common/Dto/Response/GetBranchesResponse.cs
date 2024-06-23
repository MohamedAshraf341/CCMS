using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Response
{
    public class GetBranchesResponse
    {
        public Guid Id { get; set; }
        public string Reasturant { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public List<PhoneNumberDto> PhoneNumbers { get; set; }
    }
    public class PhoneNumberDto
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }

    }
}
