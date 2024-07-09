using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Response.Client
{
    public class GetClients:BaseResponse
    {
        public List<CustomerDto>? Customers { get; set; }
    }
}
