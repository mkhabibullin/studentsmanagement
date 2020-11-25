namespace Shared.Contract.Messages
{
    public interface ValidateStudentMessage : ISagaMessage
    {
        long StudentId { get; }
        string PublicId { get; }
        string FullName { get; }
    }
}
