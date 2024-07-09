using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Dto.Request.MenuItem;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories;

public class MenuItemRepository:BaseRepository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>> GetAllWithInclude(GetMenuItems req)
    {
        var query=_context.MenuItems
            .Include(m => m.Branch)
            .ThenInclude(b => b.Restaurant)
            .Include(m => m.MenuItemOrders)
            .AsQueryable();
        if(req.BranchId.HasValue)
            query.Where(m => m.Id == req.BranchId.Value);
        var res= await query.ToListAsync();
        return res;        
    }
}
