﻿namespace CCMS.Common.Dto.Request;

public class ConfirmEmailRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}
