// <copyright file="ReactableBase.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.ObjectModel;
using Core;
using UniDirectional;

// TODO: Make some of the methods virtual

/// <summary>
/// Defines a provider for pushing notifications or receiving responses with default behavior.
/// </summary>
/// <typeparam name="T">The type of reactor to use.</typeparam>
public abstract class ReactableBase<T> : IReactable<T>
    where T : class, IReactor
{
    private readonly List<T> reactors = new ();
    private bool notificationsEnded;

    /// <inheritdoc/>
    public ReadOnlyCollection<T> Reactors => this.reactors.AsReadOnly();

    /// <inheritdoc/>
    public ReadOnlyCollection<Guid> SubscriptionIds => this.reactors
        .Select(r => r.Id)
        .Distinct()
        .ToList().AsReadOnly();

    /// <summary>
    /// Gets a value indicating whether or not if the <see cref="ReactableBase{T}"/> has been disposed.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the given <paramref name="reactor"/> is null.</exception>
    public virtual IDisposable Subscribe(T reactor)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<T>), $"{nameof(PushReactable<T>)} disposed.");
        }

        if (reactor is null)
        {
            throw new ArgumentNullException(nameof(reactor), "The parameter must not be null.");
        }

        this.reactors.Add(reactor);

        return new ReactorUnsubscriber(this.reactors.Cast<IReactor>().ToList(), reactor);
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public virtual void Unsubscribe(Guid id)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<T>), $"{nameof(PushReactable<T>)} disposed.");
        }

        if (this.notificationsEnded)
        {
            return;
        }

        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.reactors.Count - 1
                ? this.reactors.Count - 1
                : i;

            if (this.reactors[i].Id != id)
            {
                continue;
            }

            var beforeTotal = this.reactors.Count;

            this.reactors[i].OnUnsubscribe();

            var nothingRemoved = Math.Abs(beforeTotal - this.reactors.Count) <= 0;

            // Make sure that the OnUnsubscribe implementation did not remove
            // the reactor before attempting to remove it
            if (nothingRemoved)
            {
                this.reactors.Remove(this.reactors[i]);
            }
        }

        this.notificationsEnded = this.reactors.All(r => r.Unsubscribed);
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public virtual void UnsubscribeAll()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<T>), $"{nameof(PushReactable<T>)} disposed.");
        }

        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.reactors.Count - 1
                ? this.reactors.Count - 1
                : i;

            this.reactors[i].OnUnsubscribe();
        }

        this.reactors.Clear();
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
    ///     All <see cref="IReactor"/>s that are still subscribed will have its <see cref="IReactor.OnUnsubscribe"/>
    ///     method invoked and the <see cref="IReactor"/>s will be unsubscribed.
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
