using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using WorkerService1.Messages;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                CreateHostBuilder(logger, args)
                    .Build()
                    .Run();
            }
            catch (Exception exc)
            {
                logger.Fatal(exc, "App exception");
            }
        }

        public static IHostBuilder CreateHostBuilder(ILogger logger, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHostedService<WorkerClient>();
                    services.AddHostedService<WorkerClient2>();
                    services.AddHostedService<WorkerClient3>();
                    services.AddLogging(builder => builder.AddSerilog(logger, true));

                    services.AddTransient<RequestConsumer>();
                });
    }
}
