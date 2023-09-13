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
    IRespondReactor<TData> BuildOneWayRespond<TData>(Func<TData> onRespond);
    IRespondReactor<TIn, TOut> BuildTwoWayRespond<TIn, TOut>(Func<TIn, TOut> onRespond);
}
