using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class MenuItem
    {
        public Guid Id { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
        [Required]
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
        public ICollection<MenuItemOrder> MenuItemOrders { get; set; }

    }
}
