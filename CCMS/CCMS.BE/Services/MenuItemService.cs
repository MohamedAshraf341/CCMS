using CCMS.BE.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Services
{
    public class MenuItemService
    {
        private readonly IUnitOfWork _uow;

        public MenuItemService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Common.Dto.Response.MenuItem.GetMenuItems> GetMenuItems(Common.Dto.Request.MenuItem.GetMenuItems req)
        {
            try
            {
                var items=await _uow.MenuItem.GetAllWithInclude(req);
                var menuItems=items.Select(m => new Common.Dto.MenuItemDto
                {
                    Name = m.Name,
                    BranchId = m.BranchId,
                     Description = m.Description,
                     Id = m.Id,
                     Price = m.Price,   
                     Picture= m.Picture,
                     BranchName=m.Branch!=null&&m.Branch.Restaurant!=null? GetFormattedRestaurant(m.Branch.Restaurant.Name,m.Branch.Area,m.Branch.City,m.Branch.Government):string.Empty,
                     CountOrders=m.MenuItemOrders.Count,
                }).ToList();
                var res = new Common.Dto.Response.MenuItem.GetMenuItems { MenuItems=menuItems,Success=true,Message="List of Menu Items"};
                return res;
            }
            catch(Exception  ex) 
            {
                return new Common.Dto.Response.MenuItem.GetMenuItems { Message = ex.Message ,Success=false};
            }
        }
        private string GetFormattedRestaurant(string restaurantName, string Area, string City, string Government)
        {
            return $"{Area}, {City}, {Government}, {restaurantName}";
        }
    }
}
