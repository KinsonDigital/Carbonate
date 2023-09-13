// <copyright file="ReactableBase.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.ObjectModel;
using Core;
using OneWay;

/// <summary>
/// Defines a provider for pushing notifications or receiving responses with default behavior.
/// </summary>
/// <typeparam name="TSubscription">The type of reactor to use.</typeparam>
public abstract class ReactableBase<TSubscription> : IReactable<TSubscription>
    where TSubscription : class, ISubscription
{
    private readonly List<TSubscription> subscriptions = new ();
    private bool notificationsEnded;

    /// <inheritdoc/>
    public ReadOnlyCollection<TSubscription> Subscriptions => this.subscriptions.AsReadOnly();

    /// <inheritdoc/>
    public ReadOnlyCollection<Guid> SubscriptionIds => this.subscriptions
        .Select(r => r.Id)
        .Distinct()
        .ToList().AsReadOnly();

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

        this.subscriptions.Add(subscription);

        return new SubscriptionUnsubscriber(this.subscriptions.Cast<ISubscription>().ToList(), subscription);
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
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.subscriptions.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.subscriptions.Count - 1
                ? this.subscriptions.Count - 1
                : i;

            if (this.subscriptions[i].Id != id)
            {
                continue;
            }

            var beforeTotal = this.subscriptions.Count;

            this.subscriptions[i].OnUnsubscribe();

            var nothingRemoved = Math.Abs(beforeTotal - this.subscriptions.Count) <= 0;

            // Make sure that the OnUnsubscribe implementation did not remove
            // the reactor before attempting to remove it
            if (nothingRemoved)
            {
                this.subscriptions.Remove(this.subscriptions[i]);
            }
        }

        this.notificationsEnded = this.subscriptions.All(r => r.Unsubscribed);
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public virtual void UnsubscribeAll()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<TSubscription>), $"{nameof(PushReactable<TSubscription>)} disposed.");
        }

        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.subscriptions.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.subscriptions.Count - 1
                ? this.subscriptions.Count - 1
                : i;

            this.subscriptions[i].OnUnsubscribe();
        }

        this.subscriptions.Clear();
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
    private void Dispose(bool disposing)
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
}
