using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CCMS.Common.Dto.Response;

public class TokenResponse : BaseResponse
{
    public string Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    public List<string>? Roles { get; set; }
    public byte[]? Picture { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }


}
