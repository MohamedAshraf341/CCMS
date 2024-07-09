using System.Collections.Generic;
namespace CCMS.Common.Dto.Response.Order
{
    public class GetOrders:BaseResponse
    {
        public List<OrderDto>? Orders { get; set; }

    }
}
