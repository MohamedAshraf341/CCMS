using System;
using System.Collections.Generic;

namespace CCMS.Common.Dto.Response.User;

public class GetUsers: BaseResponse
{
    public List<UsersDto>? Users { get; set; }
}
