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
    private string? subName;
    private Action? unsubscribe;
    private Action<Exception>? subOnError;

    internal SubscriptionBuilder()
    {
    }

    // REQUIRED
    public ISubscriptionBuilder WithId(Guid id)
    public ISubscriptionBuilder WithId(Guid newId)
    {
        if (newId == Guid.Empty)
        {
            throw new EmptySubscriptionIdException();
        }

        this.id = newId;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        this.subName ??= name;
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

        this.subOnError ??= onError;
        return this;
    }

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

    public IRespondSubscription<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onRespond)
    public IRespondSubscription<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onReceiveRespond)
    {
        ArgumentNullException.ThrowIfNull(onReceiveRespond);

        return new RespondSubscription<TIn, TOut>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceiveRespond: onReceiveRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);
    }
}
