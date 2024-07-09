using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class Branch
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Government { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public ICollection<BranchPhone>? BranchPhones { get; set; }
        public ICollection<MenuItem>? MenuItems { get; set; }
        public ICollection<BranchUser>? BranchUsers { get; set; }


    }
}
