using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Request.MenuItem
{
    public class AddOrEditMenuItem
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
    }
}
