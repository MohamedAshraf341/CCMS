using CCMS.BE.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces
{
    public interface IUserSettingRepository : IBaseRepository<UserSetting>
    {
        Task <UserSetting> GetByUserAndKey(string userId,string Key);
        Task<IEnumerable<UserSetting>> GetByUser(string userId);

    }

}
