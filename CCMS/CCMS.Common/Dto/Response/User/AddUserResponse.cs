﻿namespace CCMS.Common.Dto.Response.User
{
    public class AddUserResponse : BaseResponse
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}
