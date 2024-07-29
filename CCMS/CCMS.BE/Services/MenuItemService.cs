using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Dto.Response;
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
        public async Task<BaseResponse> Add(Common.Dto.Request.MenuItem.AddOrEditMenuItem item)
        {
            try
            {
                var menuItem = new MenuItem
                {
                    Id = item.Id,
                    Price = item.Price,
                    BranchId = item.BranchId,
                    Description = item.Description,
                    Name = item.Name,
                    Picture = item.Picture,
                };
                await _uow.MenuItem.AddAsync(menuItem);
                var res =await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Message = "Add Menu Item Successfully .", Success = true };
                else
                    return new BaseResponse { Message ="there were some problem." ,Success = false};
            }
            catch(Exception ex) 
            {
                return new BaseResponse { Message = ex.Message ,Success=true};
            }
        }
    }
}
