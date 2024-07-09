using CCMS.Common.Const;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Client : ApiClientBase
    {
        public ApiClient_Client(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {

        }
        internal async Task<Common.Dto.Response.Client.GetClients> GetClients(Common.Dto.Request.Client.GetClients model)
        {
            var res = await ApiHttpClient.Post<Common.Dto.Request.Client.GetClients, Common.Dto.Response.Client.GetClients>(Router.Client.GetClients, model);
            return res;
        }
    }
}
