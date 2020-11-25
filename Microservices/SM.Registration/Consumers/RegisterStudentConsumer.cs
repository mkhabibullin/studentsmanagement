using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contract;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using SM.Registration.Services;
using System;
using System.Threading.Tasks;

namespace SM.Registration.Consumers
{
    public class RegisterStudentConsumer : IConsumer<RegisterStudentMessage>
    {
        private readonly IRegistrationService registrationService;
        private readonly ILogger<RegisterStudentConsumer> logger;

        public RegisterStudentConsumer(IRegistrationService registrationService, ILogger<RegisterStudentConsumer> logger)
        {
            this.registrationService = registrationService;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<RegisterStudentMessage> context)
        {
            var msg = context.Message;
            var fullName = msg.FullName;
            var studentId = msg.StudentId;

            if (fullName.StartsWith("reg", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Test exception");
            }

            try
            {
                logger.LogInformation($"Student {fullName} - {studentId} registration");

                var publicId = registrationService.Register(studentId, fullName);

                await context.Publish<StudentRegisteredEvent>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    PublicId = publicId
                });
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Student {fullName} - {studentId} registration failed");

                await context.Publish<StudentRejectedEvent>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    Reason = ex.Message
                });
            }
        }
    }
}
