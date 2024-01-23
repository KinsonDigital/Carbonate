// <copyright file="NotificationException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Exceptions;

/// <summary>
/// Thrown when something goes wrong with the notification process.
/// </summary>
public sealed class NotificationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationException"/> class.
    /// </summary>
    public NotificationException()
        : base("The send notification process is currently in progress.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotificationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public NotificationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
