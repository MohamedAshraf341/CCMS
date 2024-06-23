using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Response
{
    public class PhoneNumbersDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public Guid BranchId { get; set; }
    }
}
