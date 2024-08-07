﻿using CCMS.Common.Const;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_MenuItem : ApiClientBase
    {
        public ApiClient_MenuItem(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {

        }
        internal async Task<Common.Dto.Response.MenuItem.GetMenuItems> GetMenuItems(Common.Dto.Request.MenuItem.GetMenuItems model)
        {
            var res = await ApiHttpClient.Post<Common.Dto.Request.MenuItem.GetMenuItems, Common.Dto.Response.MenuItem.GetMenuItems>(Router.MenuItem.GetMenuItems, model);
            return res;
        }
        internal async Task<Common.Dto.Response.BaseResponse> AddMenuItem(Common.Dto.Request.MenuItem.AddOrEditMenuItem model)
        {
            var res = await ApiHttpClient.Post<Common.Dto.Request.MenuItem.AddOrEditMenuItem, Common.Dto.Response.BaseResponse>(Router.MenuItem.AddMenuItem, model);
            return res;
        }
    }
}
