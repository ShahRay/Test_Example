using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using Test_Example.Back.Services;

namespace Test_Example.Worker.Back
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);

            host.UseWindowsService();
 
            host.ConfigureAppConfiguration(
                  (hostContext, config) =>
                  {
                      config.SetBasePath(Directory.GetCurrentDirectory());
                      config.AddJsonFile("appsettings.json", false, true);
                      config.AddCommandLine(args);
                  }
            );

            host.ConfigureLogging(
                  loggingBuilder =>
                  {
                      var configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
                      var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                      loggingBuilder.AddSerilog(logger, dispose: true);
                  }
            );

            host.ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();
                services.AddSingleton(typeof(IRabbitService), typeof(RabbitService));
                ServicesConfiguration.ConfigureServices(services, hostContext.Configuration);
                services.AddHostedService<Worker>();
                services.AddApplicationInsightsTelemetryWorkerService();
            });

            return host;
        }
    }
}
