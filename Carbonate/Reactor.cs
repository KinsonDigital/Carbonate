// <copyright file="Reactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Provides a mechanism for receiving push-based notifications.
/// </summary>
public sealed class Reactor : IReactor
{
    private readonly Action? onReceive;
    private readonly Action<IMessage>? onReceiveMsg;
    private readonly Action? onCompleted;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactor"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event where the <see cref="Reactor"/> responds.</param>
    /// <param name="name">The name of the <see cref="Reactor"/>.</param>
    /// <param name="onReceive">Executed when a push notification occurs with no data.</param>
    /// <param name="onReceiveMsg">Executed when a push notification occurs with a message.</param>
    /// <param name="onCompleted">Executed when the provider has finished sending push-based notifications.</param>
    /// <param name="onError">Executed when the provider experiences an error condition.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public Reactor(
        Guid eventId,
        string name = "",
        Action? onReceive = null,
        Action<IMessage>? onReceiveMsg = null,
        Action? onCompleted = null,
        Action<Exception>? onError = null)
    {
        EventId = eventId;
        Name = string.IsNullOrEmpty(name) ? string.Empty : name;
        this.onReceive = onReceive;
        this.onReceiveMsg = onReceiveMsg;
        this.onCompleted = onCompleted;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid EventId { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Unsubscribed { get; private set; }

    /// <inheritdoc />
    public void OnReceive()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onReceive?.Invoke();
    }

    /// <inheritdoc />
    public void OnReceive(IMessage message)
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onReceiveMsg?.Invoke(message);
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

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{EventId}";
}
