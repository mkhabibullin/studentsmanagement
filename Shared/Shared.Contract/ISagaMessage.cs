using System;

namespace Shared.Contract
{
    public interface ISagaMessage
    {
        Guid CorrelationId { get; }
    }
}
