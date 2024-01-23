// <copyright file="EmptySubscriptionIdException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Exceptions;

using System;

/// <summary>
/// Thrown when there is an empty subscription ID.
/// </summary>
public sealed class EmptySubscriptionIdException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptySubscriptionIdException"/> class.
    /// </summary>
    public EmptySubscriptionIdException()
        : base("The subscription ID cannot be empty.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptySubscriptionIdException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EmptySubscriptionIdException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptySubscriptionIdException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public EmptySubscriptionIdException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
