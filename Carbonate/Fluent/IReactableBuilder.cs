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

    (IDisposable, IPushReactable<TDataIn>) BuildUniPush<TDataIn>(Action<TDataIn> receive);

    (IDisposable, IPullReactable<TDataOut>) BuildUniPull<TDataOut>(Func<TDataOut> respond);

    (IDisposable, IPushPullReactable<TDataIn, TDataOut>) BuildBiPull<TDataIn, TDataOut>(Func<TDataIn, TDataOut> respond);
}
