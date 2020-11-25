using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerServiceBroadcastReceiver.Events;
using Ws.Exp.Shared.Contracts;

namespace WorkerServiceBroadcastReceiver
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _publishEndpoint;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration config, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;

            _publishEndpoint = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(_config.GetSection("RabbitMQ").GetValue<string>("Host")), h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(
                                _config.GetSection("RabbitMQ").GetValue<string>("ServiceQueueName") + "-" + Guid.NewGuid().ToString(),
                                e =>
                                {
                                    e.PrefetchCount = 1;
                                    // e.UseMessageRetry(x => x.Interval(2, 500));

                                    //e.LoadFrom(serviceProvider);

                                    EndpointConvention.Map<IEventFired>(e.InputAddress);

                                    e.Consumer(() => serviceProvider.GetRequiredService<EventFiredConsumer>());
                                });
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _publishEndpoint.StartAsync();
        }
    }
}
