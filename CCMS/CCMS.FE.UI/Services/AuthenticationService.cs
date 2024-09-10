using CCMS.Common.Dto;
using CCMS.Common.Dto.Response.Auth;
using CCMS.Common.Helpers;
using CCMS.FE.UI.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class AuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LocalStorageService localStorageService;

        private const string COOKIE_AUTH = "AuthUser";
        private const string COOKIE_Lang = "LangUser";
        private const string COOKIE_Mode = "ModeUser";

        public AuthenticationService(IHttpContextAccessor _httpContextAccessor, LocalStorageService localStorageService)
        {
            httpContextAccessor = _httpContextAccessor;
            this.localStorageService = localStorageService;
        }

        public GetToken? GetUser()
        {
            try
            {
                var cookieValue = httpContextAccessor.HttpContext.Request.Cookies[COOKIE_AUTH];
                if (string.IsNullOrEmpty(cookieValue))
                    return null;

                var userJs = EncryptionAndDecryption.Decrypt(cookieValue);
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<GetToken>(userJs);
                return user;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.GetUser :: Unhandled exception : {ex}");
                return null;
            }
        }

        public bool SetUser(GetToken user)
        {
            try
            {
                var userJs = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var cookieValue = EncryptionAndDecryption.Encrypt(userJs);

                httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_AUTH, cookieValue);

                return true;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.SetUser :: Unhandled exception : {ex}");
            }
            return false;
        }
        public void DeleteUser()
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(COOKIE_AUTH);

        }
        public async Task<UserSettingDto?> GetUserMode()
        {
            try
            {
                var mode =await localStorageService.GetItem<UserSettingDto>(COOKIE_Mode);
                return mode;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.GetUserMode :: Unhandled exception : {ex}");
                return null;
            }
        }

        public async Task<bool> SetUserMode(UserSettingDto mode)
        {
            try
            {
                await localStorageService.SetItem<UserSettingDto>(COOKIE_Mode, mode);

                return true;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.SetUserMode :: Unhandled exception : {ex}");
            }
            return false;
        }
        public async Task DeleteUserMode()
        {
            if(GetUserMode() != null)
                await localStorageService.RemoveItem(COOKIE_Mode);

        }
        public async Task<UserSettingDto?> GetUserLang()
        {
            try
            {
                var lang = await localStorageService.GetItem<UserSettingDto>(COOKIE_Lang);
                return lang;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.GetLang :: Unhandled exception : {ex}");
                return null;
            }
        }

        public async Task<bool> SetUserLang(UserSettingDto lang)
        {
            try
            {
                await localStorageService.SetItem<UserSettingDto>(COOKIE_Lang, lang);
                return true;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.SetUserLang :: Unhandled exception : {ex}");
            }
            return false;
        }
        public async Task DeleteUserLang()
        {
            if(GetUserLang() != null)
                await localStorageService.RemoveItem(COOKIE_Lang);
        }

        public void Logout()
        {
            try
            {
                httpContextAccessor.HttpContext.Response.Cookies.Delete(COOKIE_AUTH);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.Logout :: Unhandled exception : {ex}");
            }
        }
        public bool UserHasRole(string role)
        {
            var user = GetUser();
            if (user == null || user.Roles == null)
                return false;

            return user.Roles.Any(r => r == role);
        }

        public bool UserHasOneOfRole(params string[] roles)
        {
            var user = GetUser();
            if (user == null || user.Roles == null)
                return false;

            foreach (var role in roles)
            {
                if (user.Roles.Any(r => r == role))
                    return true;
            }
            return false;
        }
        //public bool UserHasclaim(string claim)
        //{
        //    var user = GetUser();
        //    if (user == null || user.RoleClaims == null)
        //        return false;

        //    return user.RoleClaims.Any(r => r == claim);
        //}

        //public bool UserHasOneOfclaim(params string[] claims)
        //{
        //    var user = GetUser();
        //    if (user == null || user.RoleClaims == null)
        //        return false;

        //    foreach (var claim in claims)
        //    {
        //        if (user.RoleClaims.Any(r => r == claim))
        //            return true;
        //    }
        //    return false;
        //}
    }
}
