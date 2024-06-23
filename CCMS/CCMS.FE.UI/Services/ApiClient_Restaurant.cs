using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
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
        internal async Task<List<GetReasturantResponse>> GetRestaurants()
        {
            var res = await ApiHttpClient.Get<List<GetReasturantResponse>>(Router.Restaurant.GetRestaurants);
            return res;
        }
        internal async Task<BaseResponse> AddRestaurant(AddReasturantRequest model)
        {
            var res= await ApiHttpClient.Post<AddReasturantRequest, BaseResponse>(Router.Restaurant.AddRestaurant, model);
            return res;
        }
        internal async Task<BaseResponse> EditRestaurant(GetReasturantResponse model)
        {
            var res = await ApiHttpClient.Put<GetReasturantResponse, BaseResponse>(Router.Restaurant.EditRestaurant, model);
            return res;
        }
        internal async Task<BaseResponse> DeleteRestaurant(Guid id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.Restaurant.DeleteRestaurant+$"/{id}");
            return res;
        }
    }
}
