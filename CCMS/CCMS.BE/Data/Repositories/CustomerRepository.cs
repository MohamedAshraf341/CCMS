using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Dto.Request.Client;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories;

public class CustomerRepository:BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllWithInclude(GetClients req)
    {
        var query=_context.Customers
            .Include(c =>c.Branch)
            .Include(c => c.Orders)
            .AsQueryable();
        if (req.BranchId.HasValue)
            query.Where(c => c.BranchId == req.BranchId.Value);
        var res= await query.ToListAsync();
        return res;
    }
}
