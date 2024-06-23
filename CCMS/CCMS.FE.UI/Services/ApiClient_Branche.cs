using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
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
        internal async Task<List<GetBranchesResponse>> GetBranches(Guid reasturantId)
        {
            var res = await ApiHttpClient.Get<List<GetBranchesResponse>>(Router.Branche.GetBranches+ $"/{reasturantId}");
            return res;
        }
        internal async Task<BaseResponse> AddBranche(AddBrancheRequest model)
        {
            var res= await ApiHttpClient.Post<AddBrancheRequest, BaseResponse>(Router.Branche.AddBranche, model);
            return res;
        }
        internal async Task<BaseResponse> EditBranche(AddBrancheRequest model)
        {
            var res = await ApiHttpClient.Put<AddBrancheRequest, BaseResponse>(Router.Branche.EditBranche, model);
            return res;
        }
        internal async Task<BaseResponse> DeleteBranche(Guid id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.Branche.DeleteBranche+$"/{id}");
            return res;
        }
    }
}
