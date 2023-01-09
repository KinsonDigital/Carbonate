// <copyright file="ReceiveReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

using Core;
using Core.NonDirectional;

/// <inheritdoc/>
public class ReceiveReactor : IReceiveReactor
{
    private readonly Action? onReceive;
    private readonly Action? onUnsubscribe;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveReactor"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event that was pushed by a <see cref="IReactable{IReceiveReactor}"/>.</param>
    /// <param name="name">The name of the <see cref="ReceiveReactor"/>.</param>
    /// <param name="onReceive">Executed when a push notification occurs with no data.</param>
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
        Action? onReceive = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        Id = eventId;
        Name = string.IsNullOrEmpty(name) ? string.Empty : name;
        this.onReceive = onReceive;
        this.onUnsubscribe = onUnsubscribe;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid Id { get; }

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
    public void OnUnsubscribe()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onUnsubscribe?.Invoke();
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
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
