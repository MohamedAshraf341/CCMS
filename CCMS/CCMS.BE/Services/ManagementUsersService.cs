using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.BE.Settings;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using CCMS.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.BE.Services;

public class ManagementUsersService : IManagementUsersService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMailingService _mailingService;
    private readonly JwtSettings _jwt;
    private readonly IUnitOfWork _uow;
    public ManagementUsersService(IMailingService mailingService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IOptions<JwtSettings> jwt,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork uow,
         IUrlHelperFactory urlHelperFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mailingService = mailingService;
        _jwt = jwt.Value;
        _httpContextAccessor = httpContextAccessor;
        _uow = uow;
        _urlHelperFactory = urlHelperFactory;
    }
    public async Task<AddUserResponse?> AddUserAsync(AddUserRequest model)
    {
        try
        {
            if (model == null)
                return null;
            var user = new ApplicationUser
            {
                Email = model.Email,
                Name = model.Name,
            };
            var passwordUser = PasswordGenerator.GeneratePassword();
            var result = await _userManager.CreateAsync(user, passwordUser);
            if (!result.Succeeded)
                return new AddUserResponse { Success=false,Message= "There is a problem with the email or password." };
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
                return new AddUserResponse {Success=false,Message= "Succeeded in adding the user, but this role does not exist." };
            var addtoRole = await _userManager.AddToRoleAsync(user, model.Role);
            if (!addtoRole.Succeeded)
                return new AddUserResponse {Success=false,Message= "Succeeded in adding the user, but failed succeeded in adding role." };
            var jwtSecurityToken = await CreateJwtToken(user);

            var response = new AddUserResponse
            {
                Success=true,
                Message= "User added successfully",
                Email = model.Email,
                Name = model.Name,
                Password= passwordUser,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
            var confirmationToke=await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodeToken = Encoding.UTF8.GetBytes(confirmationToke);
            var newToken = WebEncoders.Base64UrlEncode(encodeToken);
            var requestScheme = _httpContextAccessor.HttpContext.Request.Scheme;
            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext());

            var confirmationLink = urlHelper.Action("ConfirmEmail", "Account", new { user.Id, newToken }, requestScheme);

            var filePath = $"{Directory.GetCurrentDirectory()}\\Templates\\WelcomeTemplate.html";
            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText
                .Replace("[name]", response.Name)
                .Replace("[email]", response.Email)
                .Replace("[password]", response.Password)
                .Replace("[confirmationLink]", confirmationLink);

            await _mailingService.SendEmailAsync(response.Email, "Welcome to our Application", mailText);
            return response;
        }
        catch (Exception ex)
        {
            return new AddUserResponse { Success = false, Message = ex.Message };
        }


    }
    public async Task<BaseResponse> AddRoleAsync(AddRoleRequest model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user is null)
                return new BaseResponse { Success = false, Message = "Invalid user." };

            if (!await _roleManager.RoleExistsAsync(model.Role))
                return new BaseResponse { Success = false, Message = "Invalid Role." };

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return new BaseResponse { Success = false, Message = "User already assigned to this role." };

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (!result.Succeeded)
                return new BaseResponse { Success = false, Message = "Something went wrong." };

            return new BaseResponse { Success = false, Message = "The role has been added to the user successfully." };
        }
        catch (Exception ex) 
        {
            return new BaseResponse { Success=false,Message = ex.Message };
        }
       
    }
    public async Task<TokenResponse> LoginAsync(LoginRequest model)
    {
        try
        {
            var authModel = new TokenResponse();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return new TokenResponse {Success=false, Message = "Email is incorrect!" };
            if(!await _userManager.CheckPasswordAsync(user, model.Password))
                return new TokenResponse { Success = false, Message = "Password is incorrect!" };

            var roles = await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);

            authModel.Message = "Login is successfuly";
            authModel.Success = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = roles.ToList();
            authModel.Picture=user.Picture;
            authModel.Name=user.Name;
            authModel.Email = user.Email;

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }


            return authModel;
        }
        catch (Exception ex)
        {
            return new TokenResponse { Success = false, Message = ex.Message };
        }
    }
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest model)
    {
        try
        {
            var authModel = new TokenResponse();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == model.Token));

            if (user == null)
                return new TokenResponse {Success=false, Message = "Invalid token" };


            var refreshToken = user.RefreshTokens.Single(t => t.Token == model.Token);

            if (!refreshToken.IsActive)
                return new TokenResponse { Success = false, Message = "Inactive token" };


            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.Success = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }
        catch (Exception ex) 
        {
            return new TokenResponse { Success = false,Message = ex.Message};
        }
    }
    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
        issuer: _jwt.Issuer,
            audience: _jwt.Audience,
        claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }
    public async Task<BaseResponse> SendVerificationCodeAsync(SendCodeToEmailRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new BaseResponse { Success = false };
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var filePath = $"{Directory.GetCurrentDirectory()}\\Templates\\CodeTemplate.html";
            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[code]", code);

            await _mailingService.SendEmailAsync(model.Email, "verification code", mailText);
            var checkVerificationCode=await _uow.VerifyCodes.GetByUserId(user.Id);
            if (checkVerificationCode == null)
            {
                var verificationCode = new VerifyCode
                {
                    Code = code,
                    CreationDate = DateTime.UtcNow,
                    UserId = user.Id,
                };
                await _uow.VerifyCodes.AddAsync(verificationCode);
                await _uow.CompleteAsync();
            }
            else
            {
                checkVerificationCode.Code = code;
                checkVerificationCode.CreationDate = DateTime.UtcNow;
                _uow.VerifyCodes.Update(checkVerificationCode);
                await _uow.CompleteAsync();
            }


            return new BaseResponse { Success = true };
        }
        catch (Exception ex)
        {
            return new BaseResponse { Success = false, Message = ex.Message };
        }
    }
    public async Task<BaseResponse> VerifyCodeAsync(VerifyCodeRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new BaseResponse { Success = false, Message = "User not found" };
            var verficationCode=await _uow.VerifyCodes.GetByUserId(user.Id);
            if(verficationCode == null)
                return new BaseResponse { Success = false, Message = "Verfication code not found" };
            if(verficationCode.Code!=model.Code)
                return new BaseResponse { Success = false, Message = "The verification code is invalid" };
            return new BaseResponse { Success = true, Message = "The verification code is valid" };
        }
        catch (Exception ex)
        {
            return new BaseResponse { Success = false, Message = ex.Message };
        }
    }
    public async Task<BaseResponse> ConfirmEmailAsync(string userId, string token)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new BaseResponse { Success = false, Message = "User not found" };
            var newToken = WebEncoders.Base64UrlDecode(token);
            var encodeToken = Encoding.UTF8.GetString(newToken);
            var result = await _userManager.ConfirmEmailAsync(user, encodeToken);
            if (!result.Succeeded) return new BaseResponse { Success = false, Message = "there were problem" };
            return new BaseResponse { Success = true, Message = "Email Confirmed" };
        }
        catch (Exception ex)
        {
            return new BaseResponse { Success = false, Message = ex.Message };
        }
    }
    public async Task<List<GetUsersResponse>> GetUsersAsync()
    {

        var users = await _userManager.Users
            .Select(u => new GetUsersResponse
            {
                UserId = u.Id,
                Name = u.Name,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result.ToList()
            })
                .ToListAsync();
        return users;
    }
    public async Task<BaseResponse> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new BaseResponse { Success = false, Message = "User not found." };
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return new BaseResponse { Success = false, Message = "there were problem." };
            return new BaseResponse { Success = true, Message = "user deleted successfully." };
        }
        catch (Exception ex)
        {
            return new BaseResponse { Success = false, Message = ex.Message };
        }
    }
    public async Task<GetUsersResponse?> GetUserByIdAsync(string userId)
    {
        var res = await _userManager.FindByIdAsync(userId);
        if (res == null)
            return null;
        var user=new GetUsersResponse {
            UserId = res.Id,
            Name = res.Name,
            Email=res.Email,
            DateOfBirth = res.DateOfBirth,
            Picture = res.Picture,
            Roles= _userManager.GetRolesAsync(res).Result.ToList() };
        return user;
    }
    public async Task<BaseResponse> UpdateUserAsync(UpdateUserRequest model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return new BaseResponse { Success = false, Message = "user is invalid." };
            user.Name = model.Name;
            user.DateOfBirth= model.DateOfBirth;
            user.Picture = model.Picture;
            var res=await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                return new BaseResponse { Success = false, Message = "faild update" };
            return new BaseResponse { Success = true, Message = "updated successfully." };
        }
        catch (Exception ex)
        {
            return new BaseResponse { Success = false, Message = ex.Message };
        }
    }
    public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new BaseResponse { Success = false, Message = "user is invalid" };
            var newPassword =  _userManager.PasswordHasher.HashPassword(user,model.Password);
            return new BaseResponse { Success = true, Message = "successfully" };
        }
        catch (Exception ex)
        {
            return new BaseResponse { Success = false,Message= ex.Message };
        }
    }
    public async Task<BaseResponse> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            return new BaseResponse { Success=false};

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
            return new BaseResponse { Success = false };

        refreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return new BaseResponse { Success = true };
    }
}
