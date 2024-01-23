// <copyright file="SubscriptionUnsubscriber.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

using Exceptions;

/// <summary>
/// A subscription unsubscriber for unsubscribing from a <see cref="IReactable{TSubscription}"/>.
/// </summary>
/// <typeparam name="TSubscription">The type of subscription to use.</typeparam>
internal sealed class SubscriptionUnsubscriber<TSubscription> : IDisposable
    where TSubscription : class, ISubscription
{
    private readonly List<TSubscription> subscriptions;
    private readonly TSubscription subscription;
    private readonly Func<bool> isProcessing;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionUnsubscriber{TSubscription}"/> class.
    /// </summary>
    /// <param name="subscriptions">The list of subscriptions.</param>
    /// <param name="subscription">The subscription that has been subscribed.</param>
    /// <param name="isProcessing">Returns the in-processing state.</param>
    internal SubscriptionUnsubscriber(List<TSubscription> subscriptions, TSubscription subscription, Func<bool> isProcessing)
    {
        ArgumentNullException.ThrowIfNull(subscriptions);
        ArgumentNullException.ThrowIfNull(subscription);
        ArgumentNullException.ThrowIfNull(isProcessing);

        this.subscriptions = subscriptions;
        this.subscription = subscription;
        this.isProcessing = isProcessing;
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
            if (this.isProcessing())
            {
                var exMsg = "The send notification process is currently in progress.";
                exMsg += $"\nThe subscription '{this.subscription.Name}' with id '{this.subscription.Id}' could not be unsubscribed.";
                throw new NotificationException(exMsg);
            }

            this.subscriptions.Remove(this.subscription);
        }

        this.isDisposed = true;
    }
}
