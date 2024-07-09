using CCMS.BE.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CCMS.BE.Interfaces;

public interface IMenuItemRepository : IBaseRepository<MenuItem>
{
    Task<IEnumerable<MenuItem>> GetAllWithInclude(Common.Dto.Request.MenuItem.GetMenuItems req); 
}
