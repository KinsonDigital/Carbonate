// <copyright file="ReactorUnsubscriber.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// A reactor unsubscriber for unsubscribing from a <see cref="PushReactable"/>.
/// </summary>
internal sealed class ReactorUnsubscriber : IDisposable
{
    private readonly List<IReactor> reactors;
    private readonly IReactor reactor;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactorUnsubscriber"/> class.
    /// </summary>
    /// <param name="reactors">The list of reactor subscriptions.</param>
    /// <param name="reactor">The reactor that has been subscribed.</param>
    internal ReactorUnsubscriber(List<IReactor> reactors, IReactor reactor)
    {
        this.reactors = reactors ?? throw new ArgumentNullException(nameof(reactors), "The parameter must not be null.");
        this.reactor = reactor ?? throw new ArgumentNullException(nameof(reactor), "The parameter must not be null.");
    }

    /// <summary>
    /// Gets the total number of current subscriptions that an <see cref="PushReactable"/> has.
    /// </summary>
    public int TotalReactors => this.reactors.Count;

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
            if (this.reactors.Contains(this.reactor))
            {
                this.reactors.Remove(this.reactor);
            }
        }

        this.isDisposed = true;
    }
}
