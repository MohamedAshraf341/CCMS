using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class MenuItemOrder
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        public Order order { get; set; }
        [Required]
        public int Number { get; set; }
    }
}
