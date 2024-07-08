using System;

namespace CCMS.Common.Dto.Request.Branch
{
    public class GetBranches
    {
        public Guid? UserId { get; set; }
        public Guid? RestaurantId { get; set; }

    }
}
