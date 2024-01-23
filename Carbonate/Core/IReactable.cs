// <copyright file="IReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

using System.Collections.Immutable;

/// <summary>
/// Defines a provider for pushing notifications or receiving responses.
/// </summary>
/// <typeparam name="TSubscription">The subscription that can react to events.</typeparam>
public interface IReactable<TSubscription> : IDisposable
    where TSubscription : class, ISubscription
{
    /// <summary>
    /// Gets the list of subscriptions that are subscribed to this <see cref="IReactable{T}"/>.
    /// </summary>
    ImmutableArray<TSubscription> Subscriptions { get; }

    /// <summary>
    /// Gets the list of subscription IDs.
    /// </summary>
    ImmutableArray<Guid> SubscriptionIds { get; }

    /// <summary>
    /// Gets the list of subscription names.
    /// </summary>
    ImmutableArray<string> SubscriptionNames { get; }

    /// <summary>
    /// Notifies the provider that an subscription is to receive notifications.
    /// </summary>
    /// <param name="subscription">The object that is to receive notifications.</param>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the <paramref name="subscription"/>.
    /// </returns>
    IDisposable Subscribe(TSubscription subscription);

    /// <summary>
    /// Unsubscribes notifications to all <see cref="ISubscription"/>s that match the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the event to end.</param>
    /// <remarks>
    ///     Will not invoke the <see cref="ISubscription"/>.<see cref="ISubscription.OnUnsubscribe"/> more than once.
    /// </remarks>
    void Unsubscribe(Guid id);

    /// <summary>
    /// Unsubscribes all of the currently subscribed subscriptions.
    /// </summary>
    void UnsubscribeAll();
}
