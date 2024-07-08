using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.Mail;

public class AddMail
{
    [Required]
    public string ToEmail { get; set; }
    [Required]
    public string Subject { get; set; }
    [Required]
    public string Body { get; set; }
    public IList<IFormFile> Attachments { get; set; }
}
