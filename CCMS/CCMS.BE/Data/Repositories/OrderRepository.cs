using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories;

public class OrderRepository:BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllWithInclude(Common.Dto.Request.Order.GetOrders req)
    {
        var query = _context.Orders
            .Include(o => o.customer)
            .Include(o => o.CreatedUser)
            .Include(o => o.ReceivedUser)
            .Include(o => o.Branch)
            .ThenInclude(o => o.Restaurant)
            .AsQueryable();
        if (req.BranchId.HasValue)
            query.Where(o => o.BranchId == req.BranchId.Value);
        if(!string.IsNullOrEmpty(req.CreatedBy))
            query.Where(o => o.CreatedBy == req.CreatedBy);
        if (!string.IsNullOrEmpty(req.ReceivedBy))
            query.Where(o => o.CreatedBy == req.ReceivedBy);
        var items=await query.ToListAsync();
        return items;
    }
}
