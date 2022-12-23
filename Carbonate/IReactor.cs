// <copyright file="IReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Provides a mechanism for receiving push-based notifications.
/// </summary>
public interface IReactor
{
    /// <summary>
    /// Gets the ID of the event where this <see cref="Reactor"/> should respond.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets a value indicating whether or not the <see cref="IReactor"/> has been unsubscribed.
    /// </summary>
    /// <remarks>
    ///     This means that the <see cref="Reactor"/> will not receive <see cref="OnNext()"/>, <see cref="OnComplete"/>,
    ///     or <see cref="OnError"/> invokes.
    /// </remarks>
    bool Unsubscribed { get; }

    /// <summary>
    /// Sends the next notification without any data to the <see cref="Reactor"/>.
    /// </summary>
    void OnNext();

    /// <summary>
    /// Sends the next notification with the given <paramref name="message"/> the <see cref="Reactor"/>.
    /// </summary>
    /// <param name="message">The notification message.</param>
    void OnNext(IMessage message);

    /// <summary>
    /// Notifies the <see cref="Reactor"/> that the provider has finished sending push-based notifications.
    /// </summary>
    /// <remarks>
    ///     Will not be invoked more than once.
    /// </remarks>
    void OnComplete();

    /// <summary>
    /// Notifies the <see cref="Reactor"/> that the provider has experiences an error condition.
    /// </summary>
    /// <param name="error">An object that provides additional information about the error.</param>
    void OnError(Exception error);
}
