using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Admin : ApiClientBase
    {
        public ApiClient_Admin(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<List<GetUsersResponse>> GetUsers(GetUsersRequest request)
        {
            var res = await ApiHttpClient.Post<GetUsersRequest, List< GetUsersResponse >>(Router.Admin.GetUsers, request);
            return res;
        }
        internal async Task<AddUserResponse> AddUser(AddUserRequest request)
        {
            var res = await ApiHttpClient.Post<AddUserRequest, AddUserResponse>(Router.Admin.AddUser, request);
            return res;
        }
        internal async Task<BaseResponse> DeleteUser(string id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.Admin.DeleteUser + $"/{id}");
            return res;
        }
    }
}
