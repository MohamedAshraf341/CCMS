using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.Auth;

public class SendCodeToEmail
{
    [Required]
    public string Email { get; set; }
}
