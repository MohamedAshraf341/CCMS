using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CCMS.Common.Dto.Response;

namespace CCMS.FE.UI.Services
{
    public class BlazorCookieLoginMiddleware
    {
        public static IDictionary<Guid, TokenResponse> Logins { get; private set; }
            = new ConcurrentDictionary<Guid, TokenResponse>();


        private readonly RequestDelegate _next;

        public BlazorCookieLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AuthenticationService authenticationService)
        {
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                var user = Logins[key];
                var result = authenticationService.SetUser(user);

                if (result)
                {
                    Logins.Remove(key);
                    var returnUrl = context.Request.Query["returnUrl"];
                    var notificationMessage = user.Message; // Assuming `user.Message` contains the notification message

                    if (string.IsNullOrEmpty(returnUrl))
                        context.Response.Redirect($"/?notificationMessage={notificationMessage}");
                    else
                        context.Response.Redirect($"{returnUrl}?notificationMessage={notificationMessage}");
                    return;
                }
                else
                {
                    context.Response.Redirect("/loginfailed");
                    return;
                }
            }
            else if (context.Request.Path == "/logout")
            {
                authenticationService.Logout();
                var returnUrl = context.Request.Query["returnUrl"];
                if (string.IsNullOrEmpty(returnUrl))
                    context.Response.Redirect("/");
                else
                    context.Response.Redirect(returnUrl);
                return;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }

}
