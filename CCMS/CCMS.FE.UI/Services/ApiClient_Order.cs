using CCMS.Common.Const;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Order : ApiClientBase
    {
        public ApiClient_Order(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {

        }
        internal async Task<Common.Dto.Response.Order.GetOrders> GetOrders(Common.Dto.Request.Order.GetOrders model)
        {
            var res = await ApiHttpClient.Post<Common.Dto.Request.Order.GetOrders, Common.Dto.Response.Order.GetOrders>(Router.Order.GetOrders, model);
            return res;
        }
    }
}
