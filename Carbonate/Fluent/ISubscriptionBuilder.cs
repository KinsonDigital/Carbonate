// <copyright file="ISubscriptionBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using Core.TwoWay;
using Core.NonDirectional;
using Core.OneWay;

/// <summary>
/// Builds subscriptions using a fluent API.
/// </summary>
public interface ISubscriptionBuilder : ISetters<ISubscriptionBuilder>, IWithIdStage<ISubscriptionBuilder>
{
    /// <summary>
    /// Builds a new subscription builder object to create subscriptions.
    /// </summary>
    /// <returns>The builder object.</returns>
    static IWithIdStage<ISubscriptionBuilder> Create() => new SubscriptionBuilder();

    /// <summary>
    /// Builds a new <see cref="IReceiveSubscription"/> subscription with the given <paramref name="onReceive"/> delegate
    /// that will be executed every time a notification is pushed from the source.
    /// </summary>
    /// <param name="onReceive">The notification delegate.</param>
    /// <returns>The built subscription.</returns>
    IReceiveSubscription BuildNonReceive(Action onReceive);

    /// <summary>
    /// Builds a new <see cref="IReceiveSubscription{TIn}"/> subscription with the given <paramref name="onReceive"/> delegate
    /// that will be executed every time a notification is pushed from the source.
    /// </summary>
    /// <param name="onReceive">The notification delegate.</param>
    /// <typeparam name="TIn">The type of data that will come in with a push notification.</typeparam>
    /// <returns>The built subscription.</returns>
    IReceiveSubscription<TIn> BuildOneWayReceive<TIn>(Action<TIn> onReceive);

    /// <summary>
    /// Builds a new <see cref="IRespondSubscription{TOut}"/> subscription with the given <paramref name="onRespond"/> delegate
    /// that will be executed every time a notification is pushed from the source.
    /// </summary>
    /// <param name="onRespond">The notification delegate.</param>
    /// <typeparam name="TOut">The type of data to return to the source that sent the notification.</typeparam>
    /// <returns>The built subscription.</returns>
    IRespondSubscription<TOut> BuildOneWayRespond<TOut>(Func<TOut> onRespond);

    /// <summary>
    /// Builds a new <see cref="IReceiveRespondSubscription{TIn,TOut}"/> subscription with the given <paramref name="onReceiveRespond"/> delegate
    /// that will be executed every time a notification is pushed from the source.
    /// </summary>
    /// <param name="onReceiveRespond">The notification delegate to receive data and respond with data.</param>
    /// <typeparam name="TIn">The type of data that will come in with a push notification.</typeparam>
    /// <typeparam name="TOut">The type of data to return to the source that sent the notification.</typeparam>
    /// <returns>The built subscription.</returns>
    IReceiveRespondSubscription<TIn, TOut> BuildTwoWay<TIn, TOut>(Func<TIn, TOut> onReceiveRespond);
}
