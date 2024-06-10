using CCMS.BE.Interfaces;
using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IManagementUsersService _managementUsersService;

        public AccountController(IManagementUsersService managementUsersService)
        {
            _managementUsersService = managementUsersService;
        }
        [HttpPost(Router.Account.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var res = await _managementUsersService.ConfirmEmailAsync(userId, token);
            return Ok(res);
        }
        [HttpPost(Router.Account.LogIn)]
        public async Task<IActionResult> LogIn(LoginRequest model)
        {
            var res=await _managementUsersService.LoginAsync(model);
            return Ok(res);
        }
        [HttpPost(Router.Account.SendVerificationCode)]
        public async Task<IActionResult> SendVerificationCode(SendCodeToEmailRequest model)
        {
            var res= await _managementUsersService.SendVerificationCodeAsync(model);
            return Ok(res);
        }
        [HttpPost(Router.Account.VerifyCode)]
        public async Task<IActionResult> VerifyCode(VerifyCodeRequest model)
        {
            var res = await _managementUsersService.VerifyCodeAsync(model);
            return Ok(res);
        }

        [HttpGet(Router.Account.GetUserById+"/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var res=await _managementUsersService.GetUserByIdAsync(id);
            return Ok(res);
        }
        [HttpPut(Router.Account.UpdateUser)]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest model)
        {
            var res=await _managementUsersService.UpdateUserAsync(model);
            return Ok(res);
        }
        [HttpGet(Router.Account.LogOut + "/{token}")]
        public async Task<IActionResult> LogOut(string token)
        {
            var res = await _managementUsersService.RevokeTokenAsync(token);
            return Ok(res);
        }
    }
}
