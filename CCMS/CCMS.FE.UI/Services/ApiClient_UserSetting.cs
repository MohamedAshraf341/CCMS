using CCMS.Common.Const;
using CCMS.Common.Dto.Request.UserSetting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient_UserSetting : ApiClientBase
    {
        public ApiClient_UserSetting(ApiHttpClient apiHttpClient) : base(apiHttpClient)
        {
        }
        internal async Task<IEnumerable<Common.Dto.UserSettingDto>> GetByUser(string userId)
        {
            var res = await ApiHttpClient.Get<IEnumerable<Common.Dto.UserSettingDto>>(Router.UserSetting.GetByUser + $"/{userId}");
            return res;
        }
        internal async Task<Common.Dto.UserSettingDto> GetByUserAndKey(GetByUserAndKey dto)
        {
            var res = await ApiHttpClient.Post<GetByUserAndKey,Common.Dto.UserSettingDto>(Router.UserSetting.GetByUserAndKey,dto);
            return res;
        }
        internal async Task<IEnumerable<Common.Dto.UserSettingDto>> GetAll()
        {
            var res = await ApiHttpClient.Get<IEnumerable<Common.Dto.UserSettingDto>>(Router.UserSetting.Prefix);
            return res;
        }
        internal async Task<Common.Dto.UserSettingDto> GetById(Guid id)
        {
            var res = await ApiHttpClient.Get<Common.Dto.UserSettingDto>(Router.UserSetting.Prefix+$"/{id}");
            return res;
        }
        internal async Task<bool> Add(Common.Dto.UserSettingDto model)
        {
            var res = await ApiHttpClient.Post<Common.Dto.UserSettingDto, bool>(Router.UserSetting.Prefix,model);
            return res;
        }
        internal async Task<bool> Edit(Common.Dto.UserSettingDto model)
        {
            var res = await ApiHttpClient.Put<Common.Dto.UserSettingDto, bool>(Router.UserSetting.Prefix, model);
            return res;
        }
        internal async Task<bool> Delete(Guid id)
        {
            var res = await ApiHttpClient.Delete<bool>(Router.UserSetting.Prefix + $"/{id}");
            return res;
        }
    }
}
