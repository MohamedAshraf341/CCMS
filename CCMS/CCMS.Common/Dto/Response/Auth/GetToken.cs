using System;
using System.Collections.Generic;

namespace CCMS.Common.Dto.Response.Auth;

public class GetToken : BaseResponse
{
    public bool IsAuthenticated { get; set; }
    public string Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    public List<string>? Roles { get; set; }
    public string SystemType { get; set; }
    public Guid BranchId { get; set; }

    public byte[]? Picture { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }


}
