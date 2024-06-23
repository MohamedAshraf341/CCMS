using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories
{
    public class RestaurantRepository: BaseRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetIncludeBranches()
        {
            var items =await _context.Restaurants.Include(x => x.Branches).ToListAsync();
            return items;
        }

        public async Task<Restaurant> GetIncludeBranches(Guid Id)
        {
            var item = await _context.Restaurants.Where(x=> x.Id==Id).Include(x => x.Branches).FirstOrDefaultAsync();
            return item;
        }
    }
}
