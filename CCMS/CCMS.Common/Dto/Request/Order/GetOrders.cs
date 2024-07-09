using System;

namespace CCMS.Common.Dto.Request.Order
{
    public class GetOrders
    {
        public Guid? BranchId { get; set; }
        public string? CreatedBy { get; set; }
        public string? ReceivedBy { get; set; }

    }
}
