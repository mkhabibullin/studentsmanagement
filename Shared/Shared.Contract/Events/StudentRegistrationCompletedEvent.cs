namespace Shared.Contract.Events
{
    public interface StudentRegistrationCompletedEvent : ISagaMessage
    {
        string PublicId { get; }
    }
}
