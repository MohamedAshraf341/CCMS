using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net;
using System;

namespace CCMS.FE.UI
{
    public class AppRouteView : RouteView
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationService AuthenticationService { get; set; }
        [Inject] IOptions<AppSettings> AppSettings { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
#if DEBUG
            if (AppSettings?.Value?.DisableAuthentication == true)
            {
                base.Render(builder);
                return;
            }
#endif //DEBUG
            try
            {
                var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
                var user = AuthenticationService.GetUser();
                if (authorize && user == null)
                {
                    Console.WriteLine($"AppRouteView.Render : User not authenticated.");
                    var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                    NavigationManager.NavigateTo($"login?returnUrl={returnUrl}");
                }
                else
                {
                    base.Render(builder);
                }
            }
            catch (Exception ex)
            {
                Log.Error("AppRouteView.Render :: Unhandled exception : " + ex.ToString());
            }
        }
    }

}
