namespace CCMS.Common.Dto.Request.Auth;

public class ResetPassword
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }

}
