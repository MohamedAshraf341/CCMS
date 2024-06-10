using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Reflection;


namespace CCMS.BE
{

    public class Program
    {
        public static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        private static Action<IConfigurationBuilder> BuildConfiguration =
                builder => builder
                    //.SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();

        public static void Main(string[] args)
        {

            Console.WriteLine($"Starting Server (Env : {EnvironmentName}) ...");

            var builder = new ConfigurationBuilder();
            BuildConfiguration(builder);


            Serilog.Log.Logger =
                    new Serilog.LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .CreateLogger();

            try
            {
                Console.WriteLine("Creating host builder ...");
                var hostBuilder = CreateHostBuilder(args, builder);

                Console.WriteLine("Building host ...");
                var host = hostBuilder.Build();

                Console.WriteLine("Running host ...");
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

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationBuilder configurationBuilder)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                        })
                        .UseConfiguration(
                            configurationBuilder
                            .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"hosting.{EnvironmentName}.json", optional: true)
                            .Build()
                        )
                        .UseStartup<Startup>();
                });

            return host;
        }

    }
}