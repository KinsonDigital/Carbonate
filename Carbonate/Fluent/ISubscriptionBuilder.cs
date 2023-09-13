// <copyright file="ISubscriptionBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Core.TwoWay;
using Core.NonDirectional;
using Core.OneWay;

public interface ISubscriptionBuilder : ISetters<ISubscriptionBuilder>, IWithIdStage<ISubscriptionBuilder>
{
    static IWithIdStage<ISubscriptionBuilder> Create() => new SubscriptionBuilder();

    IReceiveSubscription BuildNonReceive(Action onReceive);
    IReceiveSubscription<TData> BuildOneWayReceive<TData>(Action<TData> onReceive);
    IRespondSubscription<TData> BuildOneWayRespond<TData>(Func<TData> onRespond);
    IRespondSubscription<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onReceiveRespond);
}
