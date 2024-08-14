using CCMS.Common.Const;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_AppSetting : ApiClientBase
    {
        public ApiClient_AppSetting(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<IEnumerable<Common.Dto.AppSettingDto>> GetAll()
        {
            var res = await ApiHttpClient.Get<IEnumerable<Common.Dto.AppSettingDto>>(Router.AppSetting.Prefix);
            return res;
        }
        internal async Task<Common.Dto.AppSettingDto> GetById(Guid id)
        {
            var res = await ApiHttpClient.Get<Common.Dto.AppSettingDto>(Router.AppSetting.Prefix+$"/{id}");
            return res;
        }
        internal async Task<bool> Add(Common.Dto.AppSettingDto model)
        {
            var res = await ApiHttpClient.Post<Common.Dto.AppSettingDto, bool>(Router.AppSetting.Prefix,model);
            return res;
        }
        internal async Task<bool> Edit(Common.Dto.AppSettingDto model)
        {
            var res = await ApiHttpClient.Put<Common.Dto.AppSettingDto, bool>(Router.AppSetting.Prefix, model);
            return res;
        }
        internal async Task<bool> Delete(Guid id)
        {
            var res = await ApiHttpClient.Delete<bool>(Router.AppSetting.Prefix + $"/{id}");
            return res;
        }
    }
}
