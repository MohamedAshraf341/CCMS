using CCMS.BE.Interfaces;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IManagementUsersService _managementUsersService;

        public AdminController(IManagementUsersService managementUsersService)
        {
            _managementUsersService = managementUsersService;
        }
        [HttpPost(Router.Admin.GetUsers)]
        public async Task<IActionResult> GetUsers(GetUsers model)
        {
            var res = await _managementUsersService.GetUsersAsync(model);
            return Ok(res);
        }
        [HttpPost(Router.Admin.AddUser)]
        public async Task<IActionResult> AddUser(AddUser model)
        {
            var res = await _managementUsersService.AddUserAsync(model);
            return Ok(res);
        }
        [HttpDelete(Router.Admin.DeleteUser+"/{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var res = await _managementUsersService.DeleteUserAsync(id);
            return Ok(res);
        }
    }
}
