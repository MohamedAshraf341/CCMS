using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories;

public class OrderRepository:BaseRepository<VerifyCode>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

}
