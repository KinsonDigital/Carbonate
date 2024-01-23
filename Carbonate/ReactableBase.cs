// <copyright file="ReactableBase.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Core;
using OneWay;

/// <summary>
/// Defines a provider for pushing notifications or receiving responses with default behavior.
/// </summary>
/// <typeparam name="TSubscription">The type of subscription to use.</typeparam>
public abstract class ReactableBase<TSubscription> : IReactable<TSubscription>
    where TSubscription : class, ISubscription
{
    private bool notificationsEnded;

    /// <inheritdoc/>
    public ImmutableArray<TSubscription> Subscriptions => InternalSubscriptions.ToImmutableArray();

    /// <inheritdoc/>
    public ReadOnlyCollection<Guid> SubscriptionIds => InternalSubscriptions
        .Select(r => r.Id)
        .Distinct()
        .ToList().AsReadOnly();

    /// <summary>
    /// Gets the list of subscriptions that are subscribed.
    /// </summary>
    internal List<TSubscription> InternalSubscriptions { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether or not the <see cref="IReactable{TSubscription}"/> is
    /// busy processing notifications.
    /// </summary>
    protected bool IsProcessing { get; set; }

    /// <summary>
    /// Gets a value indicating whether or not if the <see cref="ReactableBase{T}"/> has been disposed.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the given <paramref name="subscription"/> is null.</exception>
    public virtual IDisposable Subscribe(TSubscription subscription)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<TSubscription>), $"{nameof(PushReactable<TSubscription>)} disposed.");
        }

        if (subscription is null)
        {
            throw new ArgumentNullException(nameof(subscription), "The parameter must not be null.");
        }

        InternalSubscriptions.Add(subscription);

        return new SubscriptionUnsubscriber<TSubscription>(InternalSubscriptions, subscription, IsProcessingNotifications);
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public virtual void Unsubscribe(Guid id)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<TSubscription>), $"{nameof(PushReactable<TSubscription>)} disposed.");
        }

        if (this.notificationsEnded)
        {
            return;
        }

        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the subscriptions list which will cause an exception.
        */
        for (var i = InternalSubscriptions.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > InternalSubscriptions.Count - 1
                ? InternalSubscriptions.Count - 1
                : i;

            if (InternalSubscriptions[i].Id != id)
            {
                continue;
            }

            var beforeTotal = InternalSubscriptions.Count;

            InternalSubscriptions[i].OnUnsubscribe();

            var nothingRemoved = Math.Abs(beforeTotal - InternalSubscriptions.Count) <= 0;

            // Make sure that the OnUnsubscribe implementation did not remove
            // the subscription before attempting to remove it
            if (nothingRemoved)
            {
                InternalSubscriptions.Remove(InternalSubscriptions[i]);
            }
        }

        this.notificationsEnded = InternalSubscriptions.TrueForAll(r => r.Unsubscribed);
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public virtual void UnsubscribeAll()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<TSubscription>), $"{nameof(PushReactable<TSubscription>)} disposed.");
        }

        foreach (TSubscription subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
        {
            subscription.OnUnsubscribe();
        }

        InternalSubscriptions.Clear();
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="disposing">Disposes managed resources when <c>true</c>.</param>
    /// <remarks>
    ///     All <see cref="ISubscription"/>s that are still subscribed will have its <see cref="ISubscription.OnUnsubscribe"/>
    ///     method invoked and the <see cref="ISubscription"/>s will be unsubscribed.
    /// </remarks>
    [SuppressMessage(
        "ReSharper",
        "VirtualMemberNeverOverridden.Global",
        Justification = "Used by library users.")]
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            UnsubscribeAll();
        }

        IsDisposed = true;
    }

    /// <summary>
    /// Sends an error to all of the subscribers that match the given <paramref name="id"/>.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="id">The ID of the event where the notification will be pushed.</param>
    protected void SendError(Exception exception, Guid id)
    {
        foreach (TSubscription subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
        {
            if (subscription.Id != id)
            {
                continue;
            }

            subscription.OnError(exception);
        }
    }

    /// <summary>
    /// Returns a value indicating whether or not the <see cref="IReactable{TSubscription}"/> is busy processing notifications.
    /// </summary>
    /// <returns>True if busy.</returns>
    private bool IsProcessingNotifications() => IsProcessing;
}
