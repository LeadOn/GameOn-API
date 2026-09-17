// <copyright file="LlmException.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.Llm.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when the LLM provider answered, but the answer cannot be used.
    /// </summary>
    public class LlmException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LlmException"/> class.
        /// </summary>
        /// <param name="message">Reason the answer is unusable.</param>
        public LlmException(string message)
            : base(message)
        {
        }
    }
}
