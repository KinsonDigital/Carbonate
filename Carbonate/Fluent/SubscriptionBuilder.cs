// <copyright file="SubscriptionBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Core.TwoWay;
using Core.NonDirectional;
using Core.OneWay;
using Exceptions;
using OneWay;
using NonDirectional;
using TwoWay;

/// <inheritdoc/>
public class SubscriptionBuilder : ISubscriptionBuilder
{
    private Guid id;
    private string? subName;
    private Action? unsubscribe;
    private Action<Exception>? subOnError;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionBuilder"/> class.
    /// </summary>
    /// <remarks>
    ///     This is required to force users to use the <see cref="ISubscriptionBuilder"/>.<see cref="ISubscriptionBuilder.Create"/> to create
    ///     new instances of the <see cref="SubscriptionBuilder"/> class.
    /// </remarks>
    internal SubscriptionBuilder()
    {
    }

    /// <summary>
    /// Uses the given <paramref name="newId"/> for the final built subscription.
    /// </summary>
    /// <param name="newId">The ID to use for the subscription.</param>
    /// <returns>The subscription builder with the <paramref name="newId"/> that will be applied to the final built subscription.</returns>
    /// <exception cref="EmptySubscriptionIdException">
    ///     Thrown if the given <paramref name="newId"/> is empty.
    /// </exception>
    public ISubscriptionBuilder WithId(Guid newId)
    {
        if (newId == Guid.Empty)
        {
            throw new EmptySubscriptionIdException();
        }

        this.id = newId;
        return this;
    }

    /// <inheritdoc/>
    public ISubscriptionBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        this.subName ??= name;
        return this;
    }

    /// <inheritdoc/>
    public ISubscriptionBuilder WhenUnsubscribing(Action onUnsubscribe)
    {
        ArgumentNullException.ThrowIfNull(onUnsubscribe);
        this.unsubscribe ??= onUnsubscribe;
        return this;
    }

    /// <inheritdoc/>
    public ISubscriptionBuilder WithError(Action<Exception> onError)
    {
        ArgumentNullException.ThrowIfNull(onError);

        this.subOnError ??= onError;
        return this;
    }

    /// <inheritdoc/>
    public IReceiveSubscription BuildNonReceive(Action onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        return new ReceiveSubscription(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);
    }

    /// <inheritdoc/>
    public IReceiveSubscription<TIn> BuildOneWayReceive<TIn>(Action<TIn> onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        return new ReceiveSubscription<TIn>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);
    }

    /// <inheritdoc/>
    public IRespondSubscription<TOut> BuildOneWayRespond<TOut>(Func<TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);

        return new RespondSubscription<TOut>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onRespond: onRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);
    }

    /// <inheritdoc/>
    public IReceiveRespondSubscription<TIn, TOut> BuildTwoWay<TIn, TOut>(Func<TIn, TOut> onReceiveRespond)
    {
        ArgumentNullException.ThrowIfNull(onReceiveRespond);

        return new ReceiveRespondSubscription<TIn, TOut>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceiveRespond: onReceiveRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);
    }
}
