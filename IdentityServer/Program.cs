using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Startup>()
            .Build();

        public static void Main(string[] args)
        {

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(Configuration.GetSection("Serilog"))
            //    .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                   .Enrich.FromLogContext()
                   .MinimumLevel.Debug()
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .WriteTo.File(@"C:\AppLogs\IdentityServer\Debug.txt", LogEventLevel.Debug, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                   .WriteTo.File(@"C:\AppLogs\IdentityServer\all.txt",  LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit:10)
                   .WriteTo.File(@"C:\AppLogs\IdentityServer\error.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                   .CreateLogger();
            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
