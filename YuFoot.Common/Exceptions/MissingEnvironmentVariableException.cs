// <copyright file="MissingEnvironmentVariableException.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace YuFoot.Common.Exceptions
{
    /// <summary>
    /// MissingEnvironmentVariableException class.
    /// </summary>
    public class MissingEnvironmentVariableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingEnvironmentVariableException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MissingEnvironmentVariableException(string message)
            : base($"\"{message}\" environment variable is invalid.")
        {
        }
    }
}
