using CCMS.Common.Const;
using CCMS.Common.Dto.Request.Restaurant;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.Reasturant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Restaurant : ApiClientBase
    {
        public ApiClient_Restaurant(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<GetReasturants> GetRestaurants()
        {
            var res = await ApiHttpClient.Get<GetReasturants>(Router.Restaurant.GetRestaurants);
            return res;
        }
        internal async Task<BaseResponse> AddRestaurant(AddOrEditRestaurant model)
        {
            var res= await ApiHttpClient.Post<AddOrEditRestaurant, BaseResponse>(Router.Restaurant.AddRestaurant, model);
            return res;
        }
        internal async Task<BaseResponse> EditRestaurant(AddOrEditRestaurant model)
        {
            var res = await ApiHttpClient.Put<AddOrEditRestaurant, BaseResponse>(Router.Restaurant.EditRestaurant, model);
            return res;
        }
        internal async Task<BaseResponse> DeleteRestaurant(Guid id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.Restaurant.DeleteRestaurant+$"/{id}");
            return res;
        }
    }
}
