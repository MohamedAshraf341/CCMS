using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
