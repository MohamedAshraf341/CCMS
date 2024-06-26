﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Request
{
    public class AddBrancheRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string Government { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public Guid RestaurantId { get; set; }
        public List<string> Phones { get; set; }
    }
}
