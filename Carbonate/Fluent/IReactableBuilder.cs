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

    (IDisposable, IPushReactable) BuildNonPush(Action receive);

    (IDisposable, IPushReactable<TIn>) BuildUniPush<TIn>(Action<TIn> receive);

    (IDisposable, IPullReactable<TOut>) BuildUniPull<TOut>(Func<TOut> respond);

    (IDisposable, IPushPullReactable<TIn, TOut>) BuildBiPull<TIn, TOut>(Func<TIn, TOut> respond);
}
