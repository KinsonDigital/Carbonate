// <copyright file="ISubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Core.BiDirectional;
using Core.NonDirectional;
using Core.UniDirectional;

public interface ISubscription : ISetters<ISubscription>, IWithIdStage<ISubscription>
{
    static IWithIdStage<ISubscription> CreateSubscription() => new Subscription();

    IReceiveReactor BuildNonReceive(Action receive);
    IReceiveReactor<TData> BuildUniReceive<TData>(Action<TData> receive);
    IRespondReactor<TData> BuildUniRespond<TData>(Func<TData> respond);
    IRespondReactor<TDataIn, TDataOut> BuildBiRespond<TDataIn, TDataOut>(Func<TDataIn, TDataOut> respond);
}
