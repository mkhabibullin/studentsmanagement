using Automatonymous;
using Microsoft.Extensions.Logging;
using Shared.Contract;
using Shared.Contract.Consts;
using Shared.Contract.Events;
using Shared.Contract.Messages;
using System.Threading.Tasks;

namespace SM.Application.Saga.StudentRegistration
{
    public class StudentRegistrationStateMachine : MassTransitStateMachine<StudentRegistrationState>
    {
        public StudentRegistrationStateMachine(ILogger<StudentRegistrationStateMachine> logger)
        {
            this.InstanceState(x => x.CurrentState);
            this.ConfigureCorrelationIds();
            Initially(
                When(StudentCreated)
                .Then(x => {
                    x.Instance.StudentId = x.Data.StudentId;
                    x.Instance.FullName = x.Data.FullName;
                })
                .Then(x => logger.LogInformation($"Student {x.Instance.StudentId} created"))
                .TransitionTo(Created)
                .ThenAsync(RegisterStudentCommand)
                );
            During(Created,
                 When(StudentRegistered)
                 .Then(x => logger.LogInformation($"Student {x.Instance.StudentId} registered"))
                 .TransitionTo(Registered)
                 .ThenAsync(ValidateStudentCommand)
                 );
            During(Registered,
                  When(StudentValidated)
                  .Then(x => logger.LogInformation($"Student {x.Instance.StudentId} validated"))
                  .TransitionTo(Validated)
                  .Then(x => logger.LogInformation($"Student {x.Instance.StudentId} completed"))
                  .TransitionTo(Completed)
                  .Finalize());
            DuringAny(
                   When(StudentRejected)
                   .Then(x => logger.LogInformation($"Student {x.Instance.StudentId} rejected! because {x.Data.Reason}"))
                   .TransitionTo(Rejected)
                   .Finalize());
        }

        private async Task ValidateStudentCommand(BehaviorContext<StudentRegistrationState, StudentRegisteredEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(ValidateStudentMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<ValidateStudentMessage>(new
            {
                CorrelationId = context.Data.CorrelationId,
                StudentId = context.Instance.StudentId,
                PublicId = context.Data.PublicId,
                FullName = context.Instance.FullName
            });
        }

        private async Task RegisterStudentCommand(BehaviorContext<StudentRegistrationState, StudentCreatedEvent> context)
        {
            var uri = QueueNames.GetMessageUri(nameof(RegisterStudentMessage));
            var sendEndpoint = await context.GetSendEndpoint(uri);
            await sendEndpoint.Send<RegisterStudentMessage>(new
            {
                CorrelationId = context.Data.CorrelationId,
                StudentId = context.Data.StudentId,
                FullName = context.Data.FullName
            });
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => StudentCreated, x => x.CorrelateById(x => x.Message.CorrelationId)
                   .SelectId(c => c.Message.CorrelationId));
            Event(() => StudentRegistered, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => StudentValidated, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => StudentRejected, x => x.CorrelateById(x => x.Message.CorrelationId));
        }

        public State Created{ get; private set; }
        public State Registered { get; private set; }
        public State Validated { get; private set; }
        public State Completed { get; private set; }
        public State Rejected { get; private set; }

        public Event<StudentCreatedEvent> StudentCreated{ get; private set; }
        public Event<StudentRegisteredEvent> StudentRegistered{ get; private set; }
        public Event<StudentValidatedEvent> StudentValidated { get; private set; }
        public Event<StudentRejectedEvent> StudentRejected { get; private set; }
    }
}
