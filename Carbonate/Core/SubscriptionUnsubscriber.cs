// <copyright file="SubscriptionUnsubscriber.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

/// <summary>
/// A subscription unsubscriber for unsubscribing from a <see cref="IReactable{TSubscription}"/>.
/// </summary>
/// <typeparam name="TSubscription">The type of subscription to use.</typeparam>
internal sealed class SubscriptionUnsubscriber<TSubscription> : IDisposable
    where TSubscription : class, ISubscription
{
    private readonly List<TSubscription> subscriptions;
    private readonly TSubscription subscription;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionUnsubscriber{TSubscription}"/> class.
    /// </summary>
    /// <param name="subscriptions">The list of subscriptions.</param>
    /// <param name="subscription">The subscription that has been subscribed.</param>
    internal SubscriptionUnsubscriber(List<TSubscription> subscriptions, TSubscription subscription)
    {
        this.subscriptions = subscriptions ?? throw new ArgumentNullException(nameof(subscriptions), "The parameter must not be null.");
        this.subscription = subscription ?? throw new ArgumentNullException(nameof(subscription), "The parameter must not be null.");
    }

    /// <summary>
    /// Gets the total number of subscriptions.
    /// </summary>
    public int TotalSubscriptions => this.subscriptions.Count;

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => Dispose(true);

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="disposing">Disposes managed resources when <c>true</c>.</param>
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.subscriptions.Remove(this.subscription);
        }

        this.isDisposed = true;
    }
}
