using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories
{
    public class BrancheRepository:BaseRepository<Branch>,IBrancheRepository
    {
        public BrancheRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Branch>> GetAll(Guid? restaurantId)
        {
            var query  = _context.Branches
                .Include(x=>x.Restaurant)
                .Include(x => x.BranchPhones).AsQueryable();
            if (restaurantId.HasValue)
                query.Where(x => x.RestaurantId == restaurantId);
            var items = await query.ToListAsync();
            return items;
        }

        public async Task<Branch> GetByIdWithInclude(Guid id)
        {
            var item = await _context.Branches.Where(x => x.Id == id)
                .Include(b =>b.Restaurant)
                .Include(b => b.BranchPhones)
                .Include(b => b.MenuItems).FirstOrDefaultAsync();
            return item;
        }
    }
}
