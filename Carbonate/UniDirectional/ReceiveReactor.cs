// <copyright file="ReceiveReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.UniDirectional;

using Core;
using Core.UniDirectional;

/// <inheritdoc cref="IReceiveReactor{TDataIn}"/>
public sealed class ReceiveReactor<TDataIn> : ReactorBase, IReceiveReactor<TDataIn>
{
    private readonly Action<IMessage<TDataIn>>? onReceiveMsg;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveReactor{T}"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event that was pushed by an <see cref="IReactable{IReceiveReactor}"/>.</param>
    /// <param name="name">The name of the <see cref="ReceiveReactor{T}"/>.</param>
    /// <param name="onReceiveMsg">Executed when a push notification occurs with a message.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the provider has finished sending push-based notifications and is unsubscribed.
    /// </param>
    /// <param name="onError">Executed when the provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public ReceiveReactor(
        Guid eventId,
        string name = "",
        Action<IMessage<TDataIn>>? onReceiveMsg = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(eventId, name, onUnsubscribe, onError) => this.onReceiveMsg = onReceiveMsg;

    /// <inheritdoc />
    public void OnReceive(IMessage<TDataIn> message)
    {
        if (Unsubscribed)
        {
            return;
        }

        if (message is null)
        {
            throw new ArgumentNullException(nameof(message), "The parameter must not be null.");
        }

        this.onReceiveMsg?.Invoke(message);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
