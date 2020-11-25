using Automatonymous;
using System;

namespace SM.Application.Saga.StudentRegistration
{
    public class StudentRegistrationState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public long StudentId { get; set; }
        public string FullName { get; set; }
    }
}
