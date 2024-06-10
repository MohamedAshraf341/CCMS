using System;

namespace CCMS.Common.Dto.Request;

public class UpdateUserRequest
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public DateTime?  DateOfBirth { get; set; }
    public byte[]? Picture { get; set;}
}
