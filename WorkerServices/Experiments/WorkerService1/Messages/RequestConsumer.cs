using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Ws.Exp.Shared.Contracts;

namespace WorkerService1.Messages
{
    public class RequestConsumer : IConsumer<ISimpleRequest>
    {
        private readonly ILogger<RequestConsumer> logger;

        public RequestConsumer(ILogger<RequestConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ISimpleRequest> context)
        {
            var message = context.Message;

            logger.LogInformation("Returning name for {0}", message.CustomerId);

            context.Respond(new SimpleResponse
            {
                CusomerName = string.Format("Customer Number {0}", message.CustomerId)
            });
        }
    }

    class SimpleResponse : ISimpleResponse
    {
        public string CusomerName { get; set; }
    }
}
