namespace Shared.Contract.Events
{
    public interface StudentCreatedEvent : ISagaMessage
    {
        long StudentId { get; }
        string FullName { get; }
    }
}
