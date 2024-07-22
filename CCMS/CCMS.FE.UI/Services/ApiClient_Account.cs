using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Dto.Request.User;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.Auth;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Account : ApiClientBase
    {
        public ApiClient_Account(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<GetToken> Login(Login request)
        {
            var res = await ApiHttpClient.Post<Login, GetToken>(Router.Account.LogIn, request);
            return res;
        }
        internal async Task<BaseResponse> LogOut(RefreshToken model)
        {
            var res = await ApiHttpClient.Post<RefreshToken,BaseResponse>(Router.Account.LogOut, model);
            return res;
        }
        internal async Task<BaseResponse> RefreshToken(RefreshToken model)
        {
            var res = await ApiHttpClient.Post<RefreshToken,BaseResponse>(Router.Account.RefreshToken, model);
            return res;
        }
        internal async Task<UsersDto> GetUserById(string id)
        {
            var res = await ApiHttpClient.Get<UsersDto>(Router.Account.GetUserById+$"/{id}");
            return res;
        }
        internal async Task<BaseResponse> UpdateUser(UpdateUser model)
        {
            var res = await ApiHttpClient.Put<UpdateUser, BaseResponse>(Router.Account.UpdateUser,model);
            return res;
        }
    }
}
