// <copyright file="Reactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Provides a mechanism for receiving push-based notifications.
/// </summary>
public sealed class Reactor : IReactor
{
    private readonly Action<IMessage>? onNext;
    private readonly Action? onCompleted;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactor"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event that the <see cref="Reactor"/> should respond to.</param>
    /// <param name="onNext">Executed when a push notification occurs.</param>
    /// <param name="onCompleted">Executed when the provider has finished sending push-based notifications.</param>
    /// <param name="onError">Executed when the provider experiences an error condition.</param>
    public Reactor(Guid eventId, Action<IMessage>? onNext = null, Action? onCompleted = null, Action<Exception>? onError = null)
    {
        EventId = eventId;
        this.onNext = onNext;
        this.onCompleted = onCompleted;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid EventId { get; }

    /// <inheritdoc />
    public bool Unsubscribed { get; private set; }

    /// <inheritdoc />
    public void OnNext(IMessage message)
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onNext?.Invoke(message);
    }

    /// <inheritdoc />
    public void OnComplete()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onCompleted?.Invoke();
        Unsubscribed = true;
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onError?.Invoke(error);
    }
}
