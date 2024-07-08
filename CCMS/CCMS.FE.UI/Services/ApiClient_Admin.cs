using CCMS.Common.Const;
using CCMS.Common.Dto.Request.User;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.User;
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
        internal async Task<Common.Dto.Response.User.GetUsers> GetUsers(Common.Dto.Request.User.GetUsers request)
        {
            var res = await ApiHttpClient.Post<Common.Dto.Request.User.GetUsers, Common.Dto.Response.User.GetUsers >(Router.Admin.GetUsers, request);
            return res;
        }
        internal async Task<AddUserResponse> AddUser(AddUser request)
        {
            var res = await ApiHttpClient.Post<AddUser, AddUserResponse>(Router.Admin.AddUser, request);
            return res;
        }
        internal async Task<BaseResponse> DeleteUser(string id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.Admin.DeleteUser + $"/{id}");
            return res;
        }
    }
}
