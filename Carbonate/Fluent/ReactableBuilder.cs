// <copyright file="ReactableBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using TwoWay;
using NonDirectional;
using OneWay;

public class ReactableBuilder
    : IReactableBuilder
{
    private Guid id;
    private string? name;
    private Action? unsubscribe;
    private Action<Exception>? onError;

    internal ReactableBuilder()
    {
    }

    public IReactableBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }

    public IReactableBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public IReactableBuilder WhenUnsubscribing(Action unsubscribe)
    {
        this.unsubscribe = unsubscribe;
        return this;
    }

    public IReactableBuilder WithError(Action<Exception> onError)
    {
        this.onError = onError;
        return this;
    }

    public (IDisposable, IPushReactable) BuildNonPush(Action receive)
    {
        var reactor = new ReceiveReactor(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceive: receive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PushReactable();

        var subscription = pushReactable.Subscribe(reactor);

        return (subscription, pushReactable);
    }

    public (IDisposable, IPushReactable<TIn>) BuildUniPush<TIn>(Action<TIn> receive)
    {
        var reactor = new ReceiveReactor<TIn>(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceiveData: receive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PushReactable<TIn>();

        var subscription = pushReactable.Subscribe(reactor);

        return (subscription, pushReactable);
    }

    public (IDisposable, IPullReactable<TOut>) BuildUniPull<TOut>(Func<TOut> respond)
    {
        var reactor = new RespondReactor<TOut>(
            respondId: this.id,
            name: this.name ?? string.Empty,
            onRespond: respond,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PullReactable<TOut>();

        var subscription = pushReactable.Subscribe(reactor);

        return (subscription, pushReactable);
    }

    public (IDisposable, IPushPullReactable<TIn, TOut>) BuildBiPull<TIn, TOut>(Func<TIn, TOut> respond)
    {
        var reactor = new RespondReactor<TIn, TOut>(
            respondId: this.id,
            name: this.name ?? string.Empty,
            onRespondData: respond,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PushPullReactable<TIn, TOut>();

        var subscription = pushReactable.Subscribe(reactor);

        return (subscription, pushReactable);
    }
}
