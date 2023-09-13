// <copyright file="Subscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Core.BiDirectional;
using Core.NonDirectional;
using Core.UniDirectional;
using UniDirectional;
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
    public ISubscriptionBuilder WhenUnsubscribing(Action unsubscribe)
    {
        this.unsubscribe ??= unsubscribe;
        return this;
    }

    // OPTIONAL
    public ISubscriptionBuilder WithError(Action<Exception> onError)
    {
        this.onError ??= onError;
        return this;
    }

    public IReceiveReactor BuildNonReceive(Action receive) =>
        // TODO: check arg for null
        new ReceiveReactor(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceive: receive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

    public IReceiveReactor<TDataIn> BuildUniReceive<TDataIn>(Action<TDataIn> receive)
    {
        // TODO: check arg for null
        return new ReceiveReactor<TDataIn>(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceiveData: receive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);
    }

    public IRespondReactor<TDataOut> BuildUniRespond<TDataOut>(Func<TDataOut> respond)
    {
        // TODO: check arg for null
        return null;
    }

    public IRespondReactor<TDataIn, TDataOut> BuildBiRespond<TDataIn, TDataOut>(Func<TDataIn, TDataOut> respond)
    {
        // TODO: check arg for null
        return null;
    }
}
