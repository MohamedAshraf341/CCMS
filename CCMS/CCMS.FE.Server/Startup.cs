using CCMS.FE.UI.Extensions;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Serilog;
using System;

namespace CCMS.FE.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.Configure<AppSettings>(Configuration);

                services.AddRazorPages();
                services.AddServerSideBlazor();
                services.AddMudServices();
                services.AddServices();
                services.AddHttpContextAccessor();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup.ConfigureServices - Unhandled exception : {ex}");
                Log.Error($"Startup.ConfigureServices - Unhandled exception : {ex}");
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseStaticFiles();

                app.UseRouting();
                ServicesExtensions.Configure(app);


                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_Host");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup.ConfigureServices - Unhandled exception : {ex}");
                Log.Error($"Startup.ConfigureServices - Unhandled exception : {ex}");
            }

        }
    }

}
