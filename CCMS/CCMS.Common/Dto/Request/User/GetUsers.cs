using System;

namespace CCMS.Common.Dto.Request.User
{
    public class GetUsers
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }
        public Guid? BranchId { get; set; }
    }
}
