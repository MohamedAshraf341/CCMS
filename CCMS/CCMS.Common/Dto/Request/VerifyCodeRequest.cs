using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request;

public class VerifyCodeRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Code { get; set; }
}
