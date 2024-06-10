using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class BranchPhone
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
