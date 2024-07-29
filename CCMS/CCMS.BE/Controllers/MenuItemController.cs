using CCMS.BE.Services;
using CCMS.Common.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly MenuItemService _menuItemService;

        public MenuItemController(MenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }
        [HttpPost(Router.MenuItem.GetMenuItems)]
        public async Task<IActionResult> GetMenuItemes(Common.Dto.Request.MenuItem.GetMenuItems model)
        {
            var items = await _menuItemService.GetMenuItems(model);
            return Ok(items);
        }
        [HttpPost(Router.MenuItem.AddMenuItem)]
        public async Task<IActionResult> AddMenuItem(Common.Dto.Request.MenuItem.AddOrEditMenuItem model)
        {
            var items = await _menuItemService.Add(model);
            return Ok(items);
        }
    }
}
