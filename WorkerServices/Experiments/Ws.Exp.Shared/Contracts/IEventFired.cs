using System;

namespace Ws.Exp.Shared.Contracts
{
    public interface IEventFired
    {
        Guid Id { get; }
        string Name { get; }
    }
}
