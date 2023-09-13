// <copyright file="IReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

using System.Collections.ObjectModel;

/// <summary>
/// Defines a provider for pushing notifications or receiving responses.
/// </summary>
/// <typeparam name="TSubscription">The reactor that can subscribed to events.</typeparam>
public interface IReactable<TSubscription> : IDisposable
    where TSubscription : class, ISubscription
{
    /// <summary>
    /// Gets the list of reactors that are subscribed to this <see cref="IReactable{T}"/>.
    /// </summary>
    ReadOnlyCollection<TSubscription> Subscriptions { get; }

    /// <summary>
    /// Gets the list of subscription IDs.
    /// </summary>
    ReadOnlyCollection<Guid> SubscriptionIds { get; }

    /// <summary>
    /// Notifies the provider that an reactor is to receive notifications.
    /// </summary>
    /// <param name="subscription">The object that is to receive notifications.</param>
    /// <returns>
    ///     A reference to an interface that allows reactors to stop receiving
    ///     notifications before the provider has finished sending them.
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
    /// Unsubscribes all of the currently subscribed reactors.
    /// </summary>
    void UnsubscribeAll();
}
