using CCMS.BE.Interfaces;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request;
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
        [HttpGet(Router.Admin.GetUsers)]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _managementUsersService.GetUsersAsync();
            return Ok(res);
        }
        [HttpPost(Router.Admin.AddUser)]
        public async Task<IActionResult> AddUser(AddUserRequest model)
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
