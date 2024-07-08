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

        public async Task<IEnumerable<Branch>> GetAll(Common.Dto.Request.Branch.GetBranches model)
        {
            var query  = _context.Branches
                .Include(x=>x.Restaurant)
                .Include(x => x.BranchPhones)
                .Include(x => x.BranchUsers)
                .ThenInclude(x => x.User)
                .AsQueryable();
            if (model.RestaurantId.HasValue)
                query.Where(x => x.RestaurantId == model.RestaurantId);
            if(model.UserId.HasValue)
            {
                query.Where(x => x.BranchUsers.Any(u => u.UserId == model.UserId));
            }
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

        public async Task<Branch> GetByUserId(Guid userId)
        {
            var query =  _context.Branches.Include(x => x.BranchUsers).AsQueryable();
            var item= await query.Where(x => x.BranchUsers.Any(u => u.UserId== userId)).FirstOrDefaultAsync();
            return item;
        }
    }
}
