// <copyright file="IReactableBuilder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

using TwoWay;
using NonDirectional;
using OneWay;

/// <summary>
/// Builds reactables using a fluent API.
/// </summary>
public interface IReactableBuilder
    : ISetters<IReactableBuilder>, IWithIdStage<IReactableBuilder>
{
    /// <summary>
    /// Builds a new reactable builder object to create reactables.
    /// </summary>
    /// <returns>The builder object.</returns>
    static IWithIdStage<IReactableBuilder> Create() => new ReactableBuilder();

    /// <summary>
    /// Builds new <see cref="IPushReactable"/> and unsubscriber objects with the given <paramref name="onReceive"/> delegate
    /// that will be executed every time the built <see cref="IPushReactable"/> pushes a notification.
    /// </summary>
    /// <param name="onReceive">The notification delegate.</param>
    /// <returns>The built reactable and unsubscriber objects.</returns>
    (IDisposable, IPushReactable) BuildPush(Action onReceive);

    /// <summary>
    /// Builds new <see cref="IPushReactable{TIn}"/> and unsubscriber objects with the given <paramref name="onReceive"/> delegate
    /// that will be executed every time the built <see cref="IPushReactable{TIn}"/> pushes a notification.
    /// </summary>
    /// <param name="onReceive">The notification delegate.</param>
    /// <typeparam name="TIn">The data that will be sent to the delegate.</typeparam>
    /// <returns>The built reactable and unsubscriber objects.</returns>
    /// <remarks>
    ///     The resulting <see cref="IPushReactable{TIn}"/> object sends data.
    /// </remarks>
    (IDisposable, IPushReactable<TIn>) BuildOneWayPush<TIn>(Action<TIn> onReceive);

    /// <summary>
    /// Builds new <see cref="IPushReactable{TOut}"/> and unsubscriber objects with the given <paramref name="onRespond"/> delegate
    /// that will be executed every time the built <see cref="IPushReactable{TOut}"/> pushes a notification.
    /// </summary>
    /// <param name="onRespond">The notification delegate.</param>
    /// <typeparam name="TOut">The data that is sent back to the sender.</typeparam>
    /// <returns>The built reactable and unsubscriber objects.</returns>
    /// <remarks>
    ///     The resulting <see cref="IPullReactable{TOut}"/> object receives data back.
    /// </remarks>
    (IDisposable, IPullReactable<TOut>) BuildOneWayPull<TOut>(Func<TOut> onRespond);

    /// <summary>
    /// Builds new <see cref="IPushPullReactable{TIn, TOut}"/> and unsubscriber objects with the given <paramref name="onReceiveRespond"/>
    /// delegate that will be executed every time the built <see cref="IPushPullReactable{TIn, TOut}"/> pushes a notification.
    /// </summary>
    /// <param name="onReceiveRespond">The notification delegate.</param>
    /// <typeparam name="TIn">The data that will be sent to the delegate.</typeparam>
    /// <typeparam name="TOut">The data that is sent back to the sender.</typeparam>
    /// <returns>The built reactable and unsubscriber objects.</returns>
    /// <remarks>
    ///     The resulting <see cref="IPushPullReactable{TIn,TOut}"/> object sends data and also receives data back.
    /// </remarks>
    (IDisposable, IPushPullReactable<TIn, TOut>) BuildTwoWayPull<TIn, TOut>(Func<TIn, TOut> onReceiveRespond);
}
