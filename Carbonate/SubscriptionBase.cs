﻿// <copyright file="SubscriptionBase.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using Core;

/// <summary>
/// Provides a mechanism for push or pull based messaging.
/// </summary>
public abstract class SubscriptionBase : ISubscription
{
    private readonly Action? onUnsubscribe;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionBase"/> class.
    /// </summary>
    /// <param name="id">The ID of the event that was pushed by an <see cref="IReactable{TSubscription}"/>.</param>
    /// <param name="name">The name of the <see cref="ISubscription"/>.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the provider has finished sending push-based notifications and is unsubscribed.
    /// </param>
    /// <param name="onError">Executed when the provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    protected SubscriptionBase(
        Guid id,
        Action? onUnsubscribe = null,
        string name = "",
        Action<Exception>? onError = null)
    {
        Id = id;
        Name = string.IsNullOrEmpty(name) ? string.Empty : name;
        this.onUnsubscribe = onUnsubscribe;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public virtual bool Unsubscribed { get; private set; }

    /// <inheritdoc />
    public virtual void OnUnsubscribe()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onUnsubscribe?.Invoke();
        Unsubscribed = true;
    }

    /// <inheritdoc />
    public virtual void OnError(Exception error)
    {
        if (Unsubscribed)
        {
            return;
        }

        if (error is null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        this.onError?.Invoke(error);
    }
}
