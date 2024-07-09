using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        public Customer? customer { get; set; }
        public Guid? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? CreatedBy { get; set; }
        public ApplicationUser? CreatedUser { get; set; }
        public string? ReceivedBy { get; set; }
        public ApplicationUser? ReceivedUser { get; set; }
        public ICollection<UpdateOrder>? UpdateOrders { get; set; }
        public ICollection<MenuItemOrder>? MenuItemOrders { get; set; }

    }
}
