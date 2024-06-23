using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Request
{
    public class AddReasturantRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
