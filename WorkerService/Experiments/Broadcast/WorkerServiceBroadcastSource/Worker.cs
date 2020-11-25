using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ws.Exp.Shared.Contracts;

namespace WorkerServiceBroadcastSource
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _publishEndpoint;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            _publishEndpoint = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(_config.GetSection("RabbitMQ").GetValue<string>("Host")), h => {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _publishEndpoint.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                Console.WriteLine("Enter a line:");
                var read = Console.ReadLine();
                await _publishEndpoint.Publish<IEventFired>(new { Id = NewId.NextGuid(), Name = $"Event Fired-{read}" });
            }
        }
    }
}
