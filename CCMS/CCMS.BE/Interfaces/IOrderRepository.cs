using CCMS.BE.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CCMS.BE.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IEnumerable<Order>> GetAllWithInclude(Common.Dto.Request.Order.GetOrders req); 
}
