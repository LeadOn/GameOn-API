// <copyright file="LlmTransientException.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Llm.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when no answer could be obtained for a reason that will pass on its own: an exhausted quota
    /// (HTTP 429), an overloaded model (HTTP 5xx), or our own throttle turning the caller away because another
    /// generation was already running. None of the three is an incident - the caller is expected to try again
    /// shortly, and the API surfaces them as 429 rather than as a failure.
    /// </summary>
    public class LlmTransientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LlmTransientException"/> class.
        /// </summary>
        /// <param name="message">Provider error payload, kept for logging.</param>
        public LlmTransientException(string message)
            : base(message)
        {
        }
    }
}
