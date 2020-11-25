using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ws.Exp.Shared.Contracts;

namespace WorkerService1
{
    public class WorkerClient : BackgroundService
    {
        private readonly ILogger<WorkerClient> _logger;
        private readonly IBusControl _busControl;
        private readonly IConfiguration _config;
        private readonly IRequestClient<ISimpleRequest> _client;

        private readonly string DisplayName;

        public WorkerClient(ILogger<WorkerClient> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            DisplayName = Guid.NewGuid().ToString();

            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(_config.GetSection("RabbitMQ").GetValue<string>("Host")), h => {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            _client = CreateRequestClient(_busControl);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting bus...");
            await _busControl.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"{ DisplayName} Enter the name: ");
                var customerId = Console.ReadLine();

                var sw = Stopwatch.StartNew();
                Response<ISimpleResponse> response = await _client.GetResponse<ISimpleResponse>(new SimpleRequest(customerId));
                sw.Stop();

                Console.WriteLine("Customer Name: {0}, dur-{1}", response.Message.CusomerName, sw.ElapsedMilliseconds.ToString());
            }
        }

        private IRequestClient<ISimpleRequest> CreateRequestClient(IBusControl busControl)
        {
            var serviceAddress = new Uri(_config.GetSection("RabbitMQ").GetValue<string>("ServiceAddress"));

            var client = busControl.CreateRequestClient<ISimpleRequest>(serviceAddress, TimeSpan.FromSeconds(10));

            return client;
        }
    }

    class SimpleRequest :
            ISimpleRequest
    {
        readonly string _customerId;
        readonly DateTime _timestamp;

        public SimpleRequest(string customerId)
        {
            _customerId = customerId;
            _timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        public string CustomerId
        {
            get { return _customerId; }
        }
    }
}
