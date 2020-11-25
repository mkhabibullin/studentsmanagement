using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contract;
using System;
using System.Threading.Tasks;
using Ws.Exp.Shared.Contracts;

namespace WorkerServiceBroadcastReceiver.Events
{
    public class EventFiredConsumer : IConsumer<ISagaMessage>
    //public class EventFiredConsumer : IConsumer<IEventFired>
    {
        private readonly ILogger<EventFiredConsumer> _logger;

        public EventFiredConsumer(ILogger<EventFiredConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISagaMessage> context)
        {
            //_logger.LogInformation(context.Message.Name);
            //await Console.Out.WriteLineAsync(context.Message.Name);
        }
    }
}
