// <copyright file="IReactableBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using TwoWay;
using NonDirectional;
using OneWay;

public interface IReactableBuilder
    : ISetters<IReactableBuilder>, IWithIdStage<IReactableBuilder>
{
    static IWithIdStage<IReactableBuilder> Create() => new ReactableBuilder();

    (IDisposable, IPushReactable) BuildPush(Action onReceive);

    (IDisposable, IPushReactable<TIn>) BuildOneWayPush<TIn>(Action<TIn> onReceive);

    (IDisposable, IPullReactable<TOut>) BuildOneWayPull<TOut>(Func<TOut> onRespond);

    (IDisposable, IPushPullReactable<TIn, TOut>) BuildTwoWayPull<TIn, TOut>(Func<TIn, TOut> onRespond);
    (IDisposable, IPushPullReactable<TIn, TOut>) BuildTwoWayPull<TIn, TOut>(Func<TIn, TOut> onReceiveRespond);
}
