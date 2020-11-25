using System;

namespace Ws.Exp.Shared.Contracts
{
    public interface ISomethingHappened
    {
        string What { get; }

        DateTime When { get; }
    }
}
