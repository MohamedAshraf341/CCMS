using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.Auth;

public class VerifyCode
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Code { get; set; }
}
