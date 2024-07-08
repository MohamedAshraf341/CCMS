using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.Phone
{
    public class AddOrEditPhone
    {
        public Guid Id { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public Guid BranchId { get; set; }
    }
}
