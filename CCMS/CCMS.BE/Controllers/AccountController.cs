using CCMS.BE.Interfaces;
using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Dto.Request.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> LogIn(Login model)
        {
            var res=await _managementUsersService.LoginAsync(model);
            if (!string.IsNullOrEmpty(res.RefreshToken))
                SetRefreshTokenInCookie(res.RefreshToken, res.RefreshTokenExpiration);
            return Ok(res);
        }
        [HttpPost(Router.Account.RefreshToken)]
        public async Task<IActionResult> RefreshToken(RefreshToken model)
        {
            model.Token = !string.IsNullOrEmpty(model.Token) ? model.Token:Request.Cookies["refreshToken"];
            var result = await _managementUsersService.RefreshTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost(Router.Account.SendVerificationCode)]
        public async Task<IActionResult> SendVerificationCode(SendCodeToEmail model)
        {
            var res= await _managementUsersService.SendVerificationCodeAsync(model);
            return Ok(res);
        }
        [HttpPost(Router.Account.VerifyCode)]
        public async Task<IActionResult> VerifyCode(VerifyCode model)
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
        public async Task<IActionResult> UpdateUser(UpdateUser model)
        {
            var res=await _managementUsersService.UpdateUserAsync(model);
            return Ok(res);
        }
        [HttpPost(Router.Account.LogOut )]
        public async Task<IActionResult> LogOut(RefreshToken model)
        {
            var token = !string.IsNullOrEmpty(model.Token)? model.Token: Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var res = await _managementUsersService.RevokeTokenAsync(token);

            if (!res.Success)
                return BadRequest("Token is invalid!");
            return Ok(res);
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
