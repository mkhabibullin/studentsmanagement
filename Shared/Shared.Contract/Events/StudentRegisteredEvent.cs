namespace Shared.Contract.Events
{
    public interface StudentRegisteredEvent : ISagaMessage
    {
        string PublicId { get; }
    }
}
