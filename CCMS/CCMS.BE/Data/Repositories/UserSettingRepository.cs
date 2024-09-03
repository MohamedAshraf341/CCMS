using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories
{
    public class UserSettingRepository : BaseRepository<UserSetting>, IUserSettingRepository
    {
        public UserSettingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserSetting>> GetByUser(string userId)
        {
            var items=await _context.UserSettings.Where(x => x.UserId == userId).ToListAsync();
            return items;
        }

        public async Task<UserSetting> GetByUserAndKey(string userId, string Key)
        {
            var item = await _context.UserSettings.Where(x => x.UserId == userId && x.Key == Key).FirstOrDefaultAsync();
            return item;
        }
    }
}
