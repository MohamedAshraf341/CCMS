using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request;

public class SendCodeToEmailRequest
{
    [Required]
    public string Email { get; set; }
}
