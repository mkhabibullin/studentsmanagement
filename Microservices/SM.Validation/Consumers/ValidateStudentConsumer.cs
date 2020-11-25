using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using SM.Validation.Services;
using System;
using System.Threading.Tasks;

namespace SM.Validation.Consumers
{
    public class ValidateStudentConsumer : IConsumer<ValidateStudentMessage>
    {
        private readonly IValidationService _validationService;
        private readonly ILogger<ValidateStudentConsumer> _logger;

        public ValidateStudentConsumer(IValidationService validationService, ILogger<ValidateStudentConsumer> logger)
        {
            _validationService = validationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValidateStudentMessage> context)
        {
            var msg = context.Message;
            var fullName = msg.FullName;
            var studentId = msg.StudentId;
            var publicId = msg.PublicId;

            if (fullName.StartsWith("val", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Test exception (validation)");
            }

            try
            {
                _logger.LogInformation($"Student {fullName} - {studentId} - {publicId} validation");

                _validationService.Validate(publicId, fullName);

                await context.Publish<StudentValidatedEvent>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    PublicId = publicId
                });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Student {fullName} - {studentId} validation failed");

                await context.Publish<StudentRejectedEvent>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    Reason = ex.Message
                });
            }
        }
    }
}
