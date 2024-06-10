using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CCMS.FE.UI.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ApiClient>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<LocalStorageService>();
            services.AddSingleton<PageHistoryState>();
            services.AddScoped<NotficationServices>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<BlazorCookieLoginMiddleware>();
        }
    }

}
