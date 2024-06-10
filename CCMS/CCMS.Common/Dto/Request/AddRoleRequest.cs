using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request;

public class AddRoleRequest
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Role { get; set; }
}
