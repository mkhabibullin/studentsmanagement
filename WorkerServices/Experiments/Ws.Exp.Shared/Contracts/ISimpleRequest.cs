﻿using System;

namespace Ws.Exp.Shared.Contracts
{
    public interface ISimpleRequest
    {
        /// <summary>
        /// When the request was created
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The customer id for the request (or whatever data you want here)
        /// </summary>
        string CustomerId { get; }
    }
}
