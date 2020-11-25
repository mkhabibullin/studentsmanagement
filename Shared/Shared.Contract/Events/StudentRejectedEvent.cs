namespace Shared.Contract.Events
{
    public interface StudentRejectedEvent : ISagaMessage
    {
        string Reason{ get; }
    }
}
