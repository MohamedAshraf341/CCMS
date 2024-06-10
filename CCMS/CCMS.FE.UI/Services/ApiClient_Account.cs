using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Account : ApiClientBase
    {
        public ApiClient_Account(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<TokenResponse> Login(LoginRequest request)
        {
            var res = await ApiHttpClient.Post<LoginRequest, TokenResponse>(Router.Account.LogIn, request);
            return res;
        }
        internal async Task<BaseResponse> LogOut(string token)
        {
            var res = await ApiHttpClient.Get<BaseResponse>(Router.Account.LogOut + $"{token}");
            return res;
        }
    }
}
