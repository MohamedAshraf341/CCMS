using System;

namespace CCMS.Common.Dto.Request.Branch
{
    public class GetBranches
    {
        public string? UserId { get; set; }
        public Guid? RestaurantId { get; set; }

    }
}
