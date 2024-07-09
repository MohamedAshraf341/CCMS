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
        public async Task<Common.Dto.Response.Order.GetOrders> GetOrder(Common.Dto.Request.Order.GetOrders request)
        {
            try
            {
                var items= await _uow.Order.GetAllWithInclude(request);
                var orders= items.Select(o => new Common.Dto.OrderDto
                {
                    Id = o.Id,
                    Status = o.Status,
                    CreatedBy=o.CreatedBy,
                    CreatedName=o.CreatedUser!=null ? o.CreatedUser.Name :string.Empty,
                    ReceivedBy = o.ReceivedBy,
                    ReceivedName = o.ReceivedUser != null ? o.ReceivedUser.Name : string.Empty,
                    CreationDate = o.CreationDate,
                    CustomerName=o.customer!=null? o.customer.Name : string.Empty,
                    CustomerAddress = o.customer != null ? o.customer.Address : string.Empty,
                    CustomerPhone = o.customer != null ? o.customer.Phone : string.Empty,
                    Restaurant=o.Branch!=null && o.Branch.Restaurant!= null ? GetFormattedRestaurant(o.Branch.Restaurant.Name,o.Branch.Area,o.Branch.City,o.Branch.Government) : string.Empty,

                }).ToList();
                var res= new Common.Dto.Response.Order.GetOrders()
                { 
                    Success = true,
                    Message = "List of orders",
                    Orders = orders
                };
                return res;

            }
            catch (Exception ex)
            {

                return new Common.Dto.Response.Order.GetOrders() { Success = false, Message = ex.Message };

            }
        }

        private string GetFormattedRestaurant(string restaurantName,string Area, string City, string Government)
        {
            return $"{Area}, {City}, {Government}, {restaurantName}";
        }
    }
}
