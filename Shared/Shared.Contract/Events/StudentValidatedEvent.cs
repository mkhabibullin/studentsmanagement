using System;

namespace Shared.Contract.Events
{
    public interface StudentValidatedEvent : ISagaMessage
    {
        string FullName { get; }
        DateTime ExpirationDate { get; }
    }
}
