using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerService1.Messages;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _busControl;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IConfiguration config, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;

            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(_config.GetSection("RabbitMQ").GetValue<string>("Host")), h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(_config.GetSection("RabbitMQ").GetValue<string>("ServiceQueueName"),
                    e => { e.Consumer(() => _serviceProvider.GetRequiredService<RequestConsumer>()); });
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting bus...");            
            await _busControl.StartAsync();
        }
    }
}
