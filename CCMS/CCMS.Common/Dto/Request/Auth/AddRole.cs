using System;
using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.Auth;

public class AddRole
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Role { get; set; }
}
