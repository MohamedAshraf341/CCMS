using System;

namespace CCMS.BE.Data.Models;

public class VerifyCode
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public DateTime CreationDate { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
