using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationService _authenticationService;

        public LanguageMiddleware(RequestDelegate next, AuthenticationService authenticationService)
        {
            _next = next;
            _authenticationService = authenticationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userLang=await _authenticationService.GetUserLang();
            string lang ;
            // If lang is null or empty, use "en-US" as the default
            if (userLang == null || string.IsNullOrEmpty(userLang.Value))
            {
                lang = "en-US";
            }
            else
            {
                lang = userLang.Value;
            }

            // Set the culture info
            var cultureInfo = new CultureInfo(lang);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            // Continue processing the request
            await _next(context);
        }
    }
}
