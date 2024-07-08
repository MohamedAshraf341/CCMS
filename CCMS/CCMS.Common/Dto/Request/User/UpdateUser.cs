using System;

namespace CCMS.Common.Dto.Request.User;

public class UpdateUser
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public byte[]? Picture { get; set; }
    public string phone { get; set; }
}
