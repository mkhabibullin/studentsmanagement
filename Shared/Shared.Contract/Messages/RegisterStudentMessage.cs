namespace Shared.Contract.Messages
{
    public interface RegisterStudentMessage : ISagaMessage
    {
        long StudentId { get; }
        string FullName { get; }
    }
}
