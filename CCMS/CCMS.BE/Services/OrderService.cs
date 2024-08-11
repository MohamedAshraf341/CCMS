using CCMS.BE.Data.Models;
using CCMS.BE.Hubs;
using CCMS.BE.Interfaces;
using CCMS.Common.Dto;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderService(IUnitOfWork uow, IHubContext<OrderHub> hubContext)
        {
            _uow = uow;
            _hubContext = hubContext;
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
                    Menus = o.MenuItemOrders!=null? o.MenuItemOrders.Select(m => new MenuItemDto
                    {
                        Id = m.Id,
                        Name = m.MenuItem.Name,
                        Description = m.MenuItem.Description,
                        Number = m.Number,
                        TotalPrice = m.Number * m.MenuItem.Price,
                        Price =  m.MenuItem.Price,
                    }).ToList():null
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

        public async Task<Common.Dto.Response.BaseResponse> Add(Common.Dto.Request.Order.AddOrder model)
        {
            try
            {
                var checkCustomerExisted = await _uow.Customer.GetByPhoneNumber(model.CustomerPhone, model.BranchId.Value);
                var customer = new Customer
                {
                    Address = model.CustomerAddress,
                    Name = model.CustomerName,
                    Phone = model.CustomerPhone,
                    BranchId = model.BranchId,
                };

                if (checkCustomerExisted == null)
                    customer = await _uow.Customer.AddAsync(customer);
                else
                {
                    checkCustomerExisted.Address = model.CustomerAddress;
                    checkCustomerExisted.Name = model.CustomerName;
                    checkCustomerExisted.Phone = model.CustomerPhone;
                    checkCustomerExisted.BranchId = model.BranchId;
                    _uow.Customer.Update(checkCustomerExisted);
                }

                var order = new Order
                {
                    Confirmed = false,
                    Status = Common.Const.Status.Hold,
                    BranchId = model.BranchId,
                    CreatedBy = model.CreatedBy,
                    CreationDate = DateTime.UtcNow,
                    Notes = model.Notes,
                    Price = model.MenuItems.Sum(x => x.Price),
                    CustomerId = checkCustomerExisted == null ? customer.Id : checkCustomerExisted.Id,
                };
                order = await _uow.Order.AddAsync(order);

                foreach (var item in model.MenuItems)
                {
                    var menuItemOrder = new MenuItemOrder
                    {
                        MenuItemId = item.Id,
                        OrderId = order.Id,
                        Number= item.Number,
                    };
                    await _uow.MenuItemOrder.AddAsync(menuItemOrder);
                }

                var res = await _uow.CompleteAsync();

                if (res > 0)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate");
                    return new Common.Dto.Response.BaseResponse { Success = true, Message = "Add Order successfully." };

                }
                else
                {
                    //await transaction.RollbackAsync();
                    return new Common.Dto.Response.BaseResponse { Success = false, Message = "There were some errors." };
                }
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                return new Common.Dto.Response.BaseResponse { Message = ex.Message, Success = false };
            }

        }

        public async Task<Common.Dto.Response.BaseResponse> ConfirmOrder(Common.Dto.Request.Order.ConfirmOrder model)
        {
            try
            {
                var item = await _uow.Order.GetByIdAsync(model.Id);
                if (item != null)
                {
                    item.Confirmed = model.Confirm;
                    item.ReceivedBy = model.ReceivedUser;
                    item.Status= model.Status;
                    var res = _uow.Order.Update(item);
                }
                var resCom = await _uow.CompleteAsync();
                if (resCom > 0)
                    return new Common.Dto.Response.BaseResponse { Success = true, Message = "Order Confirmed." };
                else
                    return new Common.Dto.Response.BaseResponse { Success = false, Message = "There were some problem." };
            }
            catch (Exception ex) 
            {
                return new Common.Dto.Response.BaseResponse { Message=ex.Message, Success = false };
            }

        }
    }
}
