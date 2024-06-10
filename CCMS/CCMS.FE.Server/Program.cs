using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;


namespace CCMS.FE.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder =
                new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Log.Logger =
                new Serilog.LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();


            try
            {
                Log.Information("Creating host builder ...");
                var hostBuilder = CreateHostBuilder(args);

                Log.Information("Building host ...");
                var host = hostBuilder.Build();

                Log.Information("Running host ...");
                host.Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception : {ex}");
                Serilog.Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

}
