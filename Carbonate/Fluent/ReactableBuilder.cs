// <copyright file="ReactableBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Exceptions;
using TwoWay;
using NonDirectional;
using OneWay;

public class ReactableBuilder : IReactableBuilder
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
        if (id == Guid.Empty)
        {
            throw new EmptySubscriptionIdException();
        }

        this.id = id;
        return this;
    }

    public IReactableBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        this.name = name;
        return this;
    }

    public IReactableBuilder WhenUnsubscribing(Action onUnsubscribe)
    {
        ArgumentNullException.ThrowIfNull(onUnsubscribe);

        this.unsubscribe = onUnsubscribe;
        return this;
    }

    public IReactableBuilder WithError(Action<Exception> onError)
    {
        ArgumentNullException.ThrowIfNull(onError);

        this.onError = onError;
        return this;
    }

    public (IDisposable, IPushReactable) BuildPush(Action onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        var subscription = new ReceiveSubscription(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PushReactable();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }

    public (IDisposable, IPushReactable<TIn>) BuildOneWayPush<TIn>(Action<TIn> onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        var subscription = new ReceiveSubscription<TIn>(
            eventId: this.id,
            name: this.name ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PushReactable<TIn>();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }

    public (IDisposable, IPullReactable<TOut>) BuildOneWayPull<TOut>(Func<TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);

        var subscription = new RespondSubscription<TOut>(
            respondId: this.id,
            name: this.name ?? string.Empty,
            onRespond: onRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PullReactable<TOut>();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }

    public (IDisposable, IPushPullReactable<TIn, TOut>) BuildTwoWayPull<TIn, TOut>(Func<TIn, TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);

        var subscription = new RespondSubscription<TIn, TOut>(
            respondId: this.id,
            name: this.name ?? string.Empty,
            onRespond: onRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.onError);

        var pushReactable = new PushPullReactable<TIn, TOut>();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }
}
