// <copyright file="Subscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Core.TwoWay;
using Core.NonDirectional;
using Core.OneWay;
using OneWay;
using NonDirectional;

public class SubscriptionBuilder
    : ISubscriptionBuilder
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
        this.id = id;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WithName(string name)
    {
        this.name ??= name;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WhenUnsubscribing(Action onUnsubscribe)
    {
        this.unsubscribe ??= unsubscribe;
        this.unsubscribe ??= onUnsubscribe;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WithError(Action<Exception> onError)
    {
        this.onError ??= onError;
        return this;
    }

    public IReceiveReactor BuildNonReceive(Action onReceive)
    {
        return new ReceiveReactor(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IReceiveReactor<TIn> BuildOneWayReceive<TIn>(Action<TIn> onReceive)
    {
        // TODO: check arg for null

        return new ReceiveReactor<TIn>(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceiveData: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IRespondReactor<TOut> BuildOneWayRespond<TOut>(Func<TOut> onRespond)
    {
        // TODO: check arg for null
        return null;
    }

    public IRespondReactor<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onRespond)
    {
        // TODO: check arg for null
        return null;
    }
}
