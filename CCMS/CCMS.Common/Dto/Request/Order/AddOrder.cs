using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Request.Order
{
    public class AddOrder
    {
        [Required]
        public string Status { get; set; }
        [Required]
        public List<Guid> MenuItemIds { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
    }
}
