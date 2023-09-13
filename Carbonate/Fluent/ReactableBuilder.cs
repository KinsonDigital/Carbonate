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
    private string? subName;
    private Action? unsubscribe;
    private Action<Exception>? subOnError;

    internal ReactableBuilder()
    {
    }

    public IReactableBuilder WithId(Guid id)
    public IReactableBuilder WithId(Guid newId)
    {
        if (newId == Guid.Empty)
        {
            throw new EmptySubscriptionIdException();
        }

        this.id = newId;
        return this;
    }

    public IReactableBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        this.subName = name;
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

        this.subOnError = onError;
        return this;
    }

    public (IDisposable, IPushReactable) BuildPush(Action onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        var subscription = new ReceiveSubscription(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);

        var pushReactable = new PushReactable();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }

    public (IDisposable, IPushReactable<TIn>) BuildOneWayPush<TIn>(Action<TIn> onReceive)
    {
        ArgumentNullException.ThrowIfNull(onReceive);

        var subscription = new ReceiveSubscription<TIn>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceive: onReceive,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);

        var pushReactable = new PushReactable<TIn>();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }

    public (IDisposable, IPullReactable<TOut>) BuildOneWayPull<TOut>(Func<TOut> onRespond)
    {
        ArgumentNullException.ThrowIfNull(onRespond);

        var subscription = new RespondSubscription<TOut>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onRespond: onRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);

        var pushReactable = new PullReactable<TOut>();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }

    public (IDisposable, IPushPullReactable<TIn, TOut>) BuildTwoWayPull<TIn, TOut>(Func<TIn, TOut> onRespond)
    public (IDisposable, IPushPullReactable<TIn, TOut>) BuildTwoWayPull<TIn, TOut>(Func<TIn, TOut> onReceiveRespond)
    {
        ArgumentNullException.ThrowIfNull(onReceiveRespond);

        var subscription = new RespondSubscription<TIn, TOut>(
            id: this.id,
            name: this.subName ?? string.Empty,
            onReceiveRespond: onReceiveRespond,
            onUnsubscribe: this.unsubscribe,
            onError: this.subOnError);

        var pushReactable = new PushPullReactable<TIn, TOut>();

        var unsubscriber = pushReactable.Subscribe(subscription);

        return (unsubscriber, pushReactable);
    }
}
