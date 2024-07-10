using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.BE.Settings;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Dto.Request.User;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.Auth;
using CCMS.Common.Dto.Response.User;
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
    public async Task<AddUserResponse?> AddUserAsync(AddUser model)
    {
        try
        {
            if (model == null)
                return null;
            var user = new ApplicationUser
            {
                Email = model.Email,
                Name = model.Name,
                UserName=model.Email
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
            var confirmationToke = await _userManager.GenerateEmailConfirmationTokenAsync(user);
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
    public async Task<BaseResponse> AddRoleAsync(AddRole model)
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
    public async Task<GetToken> LoginAsync(Login model)
    {
        try
        {
            var authModel = new GetToken();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return new GetToken {Success=false, Message = "Email is incorrect!" };
            if(!await _userManager.CheckPasswordAsync(user, model.Password))
                return new GetToken { Success = false, Message = "Password is incorrect!" };
            if(!await _userManager.IsEmailConfirmedAsync(user))
                return new GetToken { Success = false, Message = "This Email Invalid." };

            var roles = await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);

            authModel.Message = "Login is successfuly";
            authModel.Success = true;
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = roles.ToList();
            authModel.Picture=user.Picture;
            authModel.Name=user.Name;
            authModel.Email = user.Email;
            authModel.Id= user.Id;
            authModel.SystemType = user.SystemType;
            if(user.SystemType == Common.Const.SystemType.Restaurant)
            {
                var branch = await _uow.Branche.GetByUserId(Guid.Parse(user.Id));
                authModel.BranchId = branch.Id;
            }
            if(user.RefreshTokens.Count()==0)
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
              
                await _userManager.UpdateAsync(user);
                var res = await _uow.CompleteAsync();
            }
            else if (user.RefreshTokens.Any(t => t.IsActive))
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
                var res = await _uow.CompleteAsync();

            }


            return authModel;
        }
        catch (Exception ex)
        {
            return new GetToken { Success = false, Message = ex.Message };
        }
    }
    public async Task<GetToken> RefreshTokenAsync(Common.Dto.Request.Auth.RefreshToken model)
    {
        try
        {
            var authModel = new GetToken();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == model.Token));

            if (user == null)
                return new GetToken {Success=false, Message = "Invalid token" };


            var refreshToken = user.RefreshTokens.Single(t => t.Token == model.Token);

            if (!refreshToken.IsActive)
                return new GetToken { Success = false, Message = "Inactive token" };


            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            await _uow.CompleteAsync();
            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Success = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }
        catch (Exception ex) 
        {
            return new GetToken { Success = false,Message = ex.Message};
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
    private Data.Models.RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new Data.Models.RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }
    public async Task<BaseResponse> SendVerificationCodeAsync(SendCodeToEmail model)
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
                var verificationCode = new Data.Models.VerifyCode
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
    public async Task<BaseResponse> VerifyCodeAsync(Common.Dto.Request.Auth.VerifyCode model)
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
    public async Task<Common.Dto.Response.User.GetUsers> GetUsersAsync(Common.Dto.Request.User.GetUsers model)
    {
        try
        {
            var items = _userManager.Users
    .Include(x => x.BranchUsers)
    .ThenInclude(x => x.Branch)
    .ThenInclude(x => x.Restaurant).AsQueryable();
            FilterUsers(items, model);
            var users = await items.Select(u => new UsersDto
            {
                UserId = u.Id,
                Name = u.Name,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result.ToList(),
                Government = u.BranchUsers.Where(b => b.UserId == Guid.Parse(u.Id)).First().Branch.Government,
                City = u.BranchUsers.Where(b => b.UserId == Guid.Parse(u.Id)).First().Branch.City,
                Area = u.BranchUsers.Where(b => b.UserId == Guid.Parse(u.Id)).First().Branch.Area,
                Restaurant = u.BranchUsers.Where(b => b.UserId == Guid.Parse(u.Id)).First().Branch.Restaurant.Name,

            }).ToListAsync();
            var res = new Common.Dto.Response.User.GetUsers { Users = users, Success = true, Message = "List of Users" };
            return res;
        }
        catch (Exception ex)
        {
            return new Common.Dto.Response.User.GetUsers { Success = false, Message = ex.Message };
        }

    }
    private IQueryable<ApplicationUser> FilterUsers(IQueryable<ApplicationUser> items, Common.Dto.Request.User.GetUsers modelFilter)
    {
        if (!string.IsNullOrEmpty(modelFilter.UserId))
            items.Where(u => u.Id != modelFilter.UserId);
        if (!string.IsNullOrEmpty(modelFilter.Role)&& !string.IsNullOrEmpty(modelFilter.UserType))
        {
            if (modelFilter.UserType == Common.Const.SystemType.System)
                items.Where(u => u.SystemType == Common.Const.SystemType.System);
            else if(modelFilter.UserType == Common.Const.SystemType.Restaurant)
                items.Where(u => u.SystemType == Common.Const.SystemType.Restaurant);
        }
        if(modelFilter.BranchId.HasValue)
            items.Where(x => x.BranchUsers.Any(x => x.BranchId == modelFilter.BranchId.Value));

        return items;
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
    public async Task<UsersDto?> GetUserByIdAsync(string userId)
    {
        var res = await _userManager.FindByIdAsync(userId);
        if (res == null)
            return null;
        var user=new UsersDto
        {
            UserId = res.Id,
            Name = res.Name,
            Email=res.Email,
            DateOfBirth = res.DateOfBirth,
            Picture = res.Picture,
            Roles= _userManager.GetRolesAsync(res).Result.ToList() };
        return user;
    }
    public async Task<BaseResponse> UpdateUserAsync(UpdateUser model)
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
    public async Task<BaseResponse> ResetPasswordAsync(ResetPassword model)
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
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return new BaseResponse { Success = false , Message = "User Not Found" };

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return new BaseResponse { Success = false, Message = "refreshToken not active" };

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            await _uow.CompleteAsync();
            return new BaseResponse { Success = true , Message ="Log Out Successfully"};
        }
        catch(Exception ex)
        {
            return new BaseResponse { Success = false,Message=ex.Message };
        }
    }

    public async Task<bool> UserIsAdmin(string userId)
    {
        var user=await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        var roles = await _userManager.GetRolesAsync(user);
        if(roles == null) return false;
        if(roles.Contains(Common.Const.Roles.Admin.Name)) return true;
        return false;
    }
}
