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
        public List<MenuItemDto> MenuItems { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerPhone { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        public Guid? BranchId { get; set; }
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
    }
}
