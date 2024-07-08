using System.Collections.Generic;
namespace CCMS.Common.Dto.Response.Reasturant
{
    public class GetReasturants:BaseResponse
    {

        public List<RestaurantDto>? Reasturants { get; set; }
    }
}
