﻿// <copyright file="Reactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Provides a mechanism for receiving push-based notifications.
/// </summary>
public sealed class Reactor : IReactor
{
    private readonly Action? onNext;
    private readonly Action<IMessage>? onNextMsg;
    private readonly Action? onCompleted;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactor"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event where the <see cref="Reactor"/> responds.</param>
    /// <param name="onNext">Executed when a push notification occurs with no data.</param>
    /// <param name="onNextMsg">Executed when a push notification occurs with a message.</param>
    /// <param name="onCompleted">Executed when the provider has finished sending push-based notifications.</param>
    /// <param name="onError">Executed when the provider experiences an error condition.</param>
    public Reactor(
        Guid eventId,
        Action? onNext = null,
        Action<IMessage>? onNextMsg = null,
        Action? onCompleted = null,
        Action<Exception>? onError = null)
    {
        EventId = eventId;
        this.onNext = onNext;
        this.onNextMsg = onNextMsg;
        this.onCompleted = onCompleted;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid EventId { get; }

    /// <inheritdoc />
    public bool Unsubscribed { get; private set; }

    /// <inheritdoc />
    public void OnNext()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onNext?.Invoke();
    }

    /// <inheritdoc />
    public void OnNext(IMessage message)
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onNextMsg?.Invoke(message);
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
