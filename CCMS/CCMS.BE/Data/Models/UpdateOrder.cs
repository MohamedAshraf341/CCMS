using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class UpdateOrder
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        [Required]
        public Guid UpdateId { get; set; }
        public Update Update { get; set; }
    }
}
