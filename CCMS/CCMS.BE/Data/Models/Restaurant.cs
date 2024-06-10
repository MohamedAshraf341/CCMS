using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class Restaurant
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Branch> Branches { get; set; }

    }
}
