using CCMS.BE.Services;
using CCMS.Common.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost(Router.Order.GetOrders)]
        public async Task<IActionResult> Getorderes(Common.Dto.Request.Order.GetOrders model)
        {
            var items = await _orderService.GetOrder(model);
            return Ok(items);
        }
    }
}
