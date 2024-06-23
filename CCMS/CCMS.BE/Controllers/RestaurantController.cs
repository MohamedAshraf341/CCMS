using CCMS.BE.Interfaces;
using CCMS.BE.Services;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly ReasturantService _reasturantService;

        public RestaurantController(ReasturantService reasturantService)
        {
            _reasturantService = reasturantService;
        }
        [HttpGet(Router.Restaurant.GetRestaurants)]
        public async Task<IActionResult> GetRestaurants()
        {
            var items = await _reasturantService.GetReasturant();
            return Ok(items);
        }
        [HttpPost(Router.Restaurant.AddRestaurant)]
        public async Task<IActionResult> AddRestaurant(AddReasturantRequest model)
        {
            var item = await _reasturantService.AddReasturant(model);
            return Ok(item);
        }
        [HttpPut(Router.Restaurant.EditRestaurant)]
        public async Task<IActionResult> EditRestaurant(GetReasturantResponse model)
        {
            var item = await _reasturantService.EditReasturant(model);
            return Ok(item);
        }
        [HttpDelete(Router.Restaurant.DeleteRestaurant+"/{id}")]
        public async Task<IActionResult> DeleteRestaurant(Guid id)
        {
            var item = await _reasturantService.DeleteReasturant(id);
            return Ok(item);
        }
    }
}
