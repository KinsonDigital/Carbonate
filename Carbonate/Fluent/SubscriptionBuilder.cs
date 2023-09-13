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

    public IReceiveReactor BuildNonReceive(Action onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        return new ReceiveReactor(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IReceiveReactor<TIn> BuildOneWayReceive<TIn>(Action<TIn> onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        return new ReceiveReactor<TIn>(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceiveData: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IRespondReactor<TOut> BuildOneWayRespond<TOut>(Func<TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);
        return null;
    }

    public IRespondReactor<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);
        return null;
    }
}
