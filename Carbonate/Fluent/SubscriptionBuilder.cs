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

public class SubscriptionBuilder : ISubscriptionBuilder
{
    private Guid id;
    private string? name;
    private Action? unsubscribe;
    private Action<Exception>? onError;

    internal SubscriptionBuilder()
    {
    }

    // REQUIRED
    public ISubscriptionBuilder WithId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new EmptySubscriptionIdException();
        }

        this.id = id;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        this.name ??= name;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WhenUnsubscribing(Action onUnsubscribe)
    {
        ArgumentNullException.ThrowIfNull(onUnsubscribe);
        this.unsubscribe ??= onUnsubscribe;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WithError(Action<Exception> onError)
    {
        ArgumentNullException.ThrowIfNull(onError);

        this.onError ??= onError;
        return this;
    }

    public IReceiveSubscription BuildNonReceive(Action onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        return new ReceiveSubscription(
            id: this.id,
            name: this.name ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IReceiveSubscription<TIn> BuildOneWayReceive<TIn>(Action<TIn> onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        return new ReceiveSubscription<TIn>(
            id: this.id,
            name: this.name ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IRespondSubscription<TOut> BuildOneWayRespond<TOut>(Func<TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);

        return new RespondSubscription<TOut>(
            respondId: this.id,
            name: this.name ?? string.Empty,
            onRespond: onRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IRespondSubscription<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);

        return new RespondSubscription<TIn, TOut>(
            respondId: this.id,
            name: this.name ?? string.Empty,
            onRespond: onRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }
}
