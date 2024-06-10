using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _uow;

        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        //public async Task<Common.Dto.BaseResponse> AddOrder(Common.Dto.Request.AddOrderRequest request)
        //{
        //   try
        //    {
        //        var model = new Order
        //        {
        //            CreationDate = DateTime.UtcNow,
        //            Status=request.Status,
        //            MenuItemOrders = request.MenuItemIds.Select(menuItemId => new MenuItemOrder
        //            {
        //                MenuItemId = menuItemId,
        //            }).ToList()

        //        };

        //    }
        //    catch (Exception ex) 
        //    {
                
        //        return new Common.Dto.BaseResponse() { Success = false,Message= ex.Message };

        //    }
        //}
    }
}
