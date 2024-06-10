using CCMS.Common.Dto.Response;
using CCMS.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCMS.FE.UI.Services
{
    public class AuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private const string Key = "Rte2bR+DY77y9aEuAPenVVCLkPSN55rTGyA+swfhykA=";
        private const string KeyIVBase64 = "MwND80W9V8urVLhK+iRKzQ==";
        private const string COOKIE_NAME = "AuthUser";
        public AuthenticationService(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }
        public string Encrypt(string input)
        {
            var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var encryptedText = symmetricEncryptDecrypt.Encrypt(input, KeyIVBase64, Key);
            return encryptedText;
        }

        public string Decrypt(string encryptedText)
        {
            var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var decryptedText = symmetricEncryptDecrypt.Decrypt(encryptedText, KeyIVBase64, Key);
            return decryptedText;
        }
        public TokenResponse? GetUser()
        {
            try
            {
                var cookieValue = httpContextAccessor.HttpContext.Request.Cookies[COOKIE_NAME];
                if (string.IsNullOrEmpty(cookieValue))
                    return null;

                var userJs = Decrypt(cookieValue);
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(userJs);
                return user;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.GetUser :: Unhandled exception : {ex}");
                return null;
            }
        }

        public bool SetUser(TokenResponse user)
        {
            try
            {
                var userJs = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var cookieValue = Encrypt(userJs);

                httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_NAME, cookieValue);

                return true;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"AuthenticationService.SetUser :: Unhandled exception : {ex}");
            }
            return false;
        }

        public void Logout()
        {
            try
            {
                httpContextAccessor.HttpContext.Response.Cookies.Delete(COOKIE_NAME);
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
