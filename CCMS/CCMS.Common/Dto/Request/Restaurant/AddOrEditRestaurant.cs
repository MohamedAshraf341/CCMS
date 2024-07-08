using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.Restaurant
{
    public class AddOrEditRestaurant
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
