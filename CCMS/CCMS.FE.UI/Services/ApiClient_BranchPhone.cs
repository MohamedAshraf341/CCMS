using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Request.Phone;
using CCMS.Common.Dto.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_BranchPhone : ApiClientBase
    {
        public ApiClient_BranchPhone(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<List<PhoneNumberDto>> GetBranchPhones(Guid branchId)
        {
            var res = await ApiHttpClient.Get<List<PhoneNumberDto>>(Router.BranchPhone.GetBranchPhones+ $"/{branchId}");
            return res;
        }
        internal async Task<BaseResponse> AddBranchPhone(AddOrEditPhone model)
        {
            var res= await ApiHttpClient.Post<AddOrEditPhone, BaseResponse>(Router.BranchPhone.AddBranchPhone, model);
            return res;
        }
        internal async Task<BaseResponse> EditBranchPhone(AddOrEditPhone model)
        {
            var res = await ApiHttpClient.Put<AddOrEditPhone, BaseResponse>(Router.BranchPhone.EditBranchPhone, model);
            return res;
        }
        internal async Task<BaseResponse> DeleteBranchPhone(Guid id)
        {
            var res = await ApiHttpClient.Delete<BaseResponse>(Router.BranchPhone.DeleteBranchPhone+$"/{id}");
            return res;
        }
    }
}
