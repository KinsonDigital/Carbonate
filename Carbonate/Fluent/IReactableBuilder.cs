// <copyright file="IReactableBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using BiDirectional;
using NonDirectional;
using UniDirectional;

public interface IReactableBuilder
    : ISetters<IReactableBuilder>, IWithIdStage<IReactableBuilder>
{
    static IWithIdStage<IReactableBuilder> Create() => new ReactableBuilder();

    (IDisposable, IPushReactable) BuildNonPush(Action receive);

    (IDisposable, IPushReactable<TDataIn>) BuildUniPush<TDataIn>(Action<TDataIn> receive);

    (IDisposable, IPullReactable<TDataOut>) BuildUniPull<TDataOut>(Func<TDataOut> respond);

    (IDisposable, IPullReactable<TDataIn, TDataOut>) BuildBiPull<TDataIn, TDataOut>(Func<TDataIn, TDataOut> respond);
}
