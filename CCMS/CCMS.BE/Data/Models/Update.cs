using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.BE.Data.Models
{
    public class Update
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }
        public ICollection<UpdateOrder> UpdateOrders { get; set; }
    }
}
