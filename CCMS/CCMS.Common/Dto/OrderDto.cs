using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedName { get; set; }
        public string? ReceivedBy { get; set; }
        public string? ReceivedName { get; set; }
        public string? Restaurant { get; set; }

    }
}
