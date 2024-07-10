using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Dto.Request.User;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.Auth;
using CCMS.Common.Dto.Response.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces;

public interface IManagementUsersService
{
    Task<AddUserResponse?> AddUserAsync(AddUser model);
    Task<BaseResponse> AddRoleAsync(AddRole model);
    Task<GetToken> LoginAsync (Login model);
    Task<GetToken> RefreshTokenAsync(RefreshToken model);
    Task<BaseResponse> SendVerificationCodeAsync(SendCodeToEmail model);
    Task<BaseResponse> VerifyCodeAsync(VerifyCode model);
    Task<BaseResponse> ConfirmEmailAsync(string userId, string token);
    Task<Common.Dto.Response.User.GetUsers> GetUsersAsync(Common.Dto.Request.User.GetUsers model);
    Task<UsersDto?> GetUserByIdAsync(string userId);
    Task<BaseResponse> DeleteUserAsync(string userId);
    Task<BaseResponse> UpdateUserAsync(UpdateUser model);
    Task<BaseResponse> ResetPasswordAsync(ResetPassword model);
    Task<BaseResponse> RevokeTokenAsync(string token);
    Task<bool> UserIsAdmin(string userId);
}
