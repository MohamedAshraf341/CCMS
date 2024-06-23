using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces;

public interface IManagementUsersService
{
    Task<AddUserResponse?> AddUserAsync(AddUserRequest model);
    Task<BaseResponse> AddRoleAsync(AddRoleRequest model);
    Task<TokenResponse> LoginAsync (LoginRequest model);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest model);
    Task<BaseResponse> SendVerificationCodeAsync(SendCodeToEmailRequest model);
    Task<BaseResponse> VerifyCodeAsync(VerifyCodeRequest model);
    Task<BaseResponse> ConfirmEmailAsync(string userId, string token);
    Task<List<GetUsersResponse>> GetUsersAsync(Common.Dto.Request.GetUsersRequest model);
    Task<GetUsersResponse?> GetUserByIdAsync(string userId);
    Task<BaseResponse> DeleteUserAsync(string userId);
    Task<BaseResponse> UpdateUserAsync(UpdateUserRequest model);
    Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest model);
    Task<BaseResponse> RevokeTokenAsync(string token);
}
