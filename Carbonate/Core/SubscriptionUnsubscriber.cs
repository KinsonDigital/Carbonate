// <copyright file="ReactorUnsubscriber.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

/// <summary>
/// A reactor unsubscriber for unsubscribing from a <see cref="IReactable{TReactor}"/>.
/// </summary>
internal sealed class SubscriptionUnsubscriber : IDisposable
{
    private readonly List<ISubscription> subscriptions;
    private readonly ISubscription subscription;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactorUnsubscriber"/> class.
    /// </summary>
    /// <param name="reactors">The list of reactor subscriptions.</param>
    /// <param name="subscription">The reactor that has been subscribed.</param>
    internal SubscriptionUnsubscriber(List<ISubscription> subscriptions, ISubscription subscription)
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
            if (this.subscriptions.Contains(this.subscription))
            {
                this.subscriptions.Remove(this.subscription);
            }
        }

        this.isDisposed = true;
    }
}
