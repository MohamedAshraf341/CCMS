using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Request.Order
{
    public class ConfirmOrder
    {
        public Guid Id { get; set; }
        public bool Confirm { get; set; }
        public string ReceivedUser { get; set; }
        public string Status { get; set; }
    }
}
