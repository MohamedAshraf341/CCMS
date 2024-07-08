using CCMS.Common.Const;
using CCMS.Common.Dto.Request.Branch;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.Branch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_Branche : ApiClientBase
    {
        public ApiClient_Branche(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<Common.Dto.Response.Branch.GetBranches> GetBranches(Common.Dto.Request.Branch.GetBranches model)
        {
            var res = await ApiHttpClient.Post< Common.Dto.Request.Branch.GetBranches , Common.Dto.Response.Branch.GetBranches > (Router.Branche.GetBranches,model );
            return res;
        }
        internal async Task<BaseResponse> AddBranche(AddOrEditBranche model)
        {
            var res= await ApiHttpClient.Post<AddOrEditBranche, BaseResponse>(Router.Branche.AddBranche, model);
            return res;
        }
        internal async Task<BaseResponse> EditBranche(AddOrEditBranche model)
        {
            var res = await ApiHttpClient.Put<AddOrEditBranche, BaseResponse>(Router.Branche.EditBranche, model);
            return res;
        }
        internal async Task<BaseResponse> DeleteBranche(Guid id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.Branche.DeleteBranche+$"/{id}");
            return res;
        }
    }
}
