// <copyright file="ReactableExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Runtime.CompilerServices;
using Fluent;
using NonDirectional;
using OneWay;

/// <summary>
/// Provides extension methods for creating subscriptions.
/// </summary>
public static class ReactableExtensionMethods
{
    /// <summary>
    /// Creates a non-directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="name">The name of the subscription.</param>
    /// <param name="onReceive">The action to execute when receiving a notification.</param>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>No data travels to or from the subscription.</remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown if the following param are empty:
    ///     <list type="bullet">
    ///         <item><paramref name="id"/></item>
    ///         <item><paramref name="name"/></item>
    ///     </list>
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the following are null:
    ///     <list type="bullet">
    ///         <item><paramref name="reactable"/></item>
    ///         <item><paramref name="name"/></item>
    ///         <item><paramref name="onReceive"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateNonReceiveOrRespond(
        this IPushReactable reactable,
        Guid id,
        string name,
        Action onReceive)
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(onReceive);

        name = name.Trim();

        var projOpenedSub = ISubscriptionBuilder.Create()
            .WithId(id)
            .WithName(name)
            .BuildNonReceiveOrRespond(onReceive);

        return reactable.Subscribe(projOpenedSub);
    }

    /// <summary>
    /// Creates a non-directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="onReceive">The action to execute when receiving a notification.</param>
    /// <param name="callerMemberName">The name of the caller.</param>
    /// <param name="callerFilePath">The file path of the caller.</param>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>No data travels to or from the subscription.</remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="id"/> param is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the following are null:
    ///     <list type="bullet">
    ///         <item><paramref name="reactable"/></item>
    ///         <item><paramref name="onReceive"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateNonReceiveOrRespond(
        this IPushReactable reactable,
        Guid id,
        Action onReceive,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(onReceive);

        callerMemberName = string.IsNullOrEmpty(callerMemberName) ? string.Empty : callerMemberName;
        callerFilePath = string.IsNullOrEmpty(callerFilePath) ? string.Empty : callerFilePath;

        callerMemberName = callerMemberName.Trim();
        callerFilePath = callerFilePath.Trim();

        callerMemberName = callerMemberName.Contains('.')
            ? callerMemberName.Split('.')[^1]
            : callerMemberName;

        var className = Path.GetFileNameWithoutExtension(callerFilePath);
        var callerName = $"{className}.{callerMemberName}";
        var subName = $"{callerName}() - {id}";

        var projOpenedSub = ISubscriptionBuilder.Create()
            .WithId(id)
            .WithName(subName)
            .BuildNonReceiveOrRespond(onReceive);

        return reactable.Subscribe(projOpenedSub);
    }

    /// <summary>
    /// Creates a one-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="name">The name of the subscription.</param>
    /// <param name="onReceive">The action to execute when receiving a notification.</param>
    /// <typeparam name="TIn">The type of data coming from the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The direction of travel that the data takes is from the source.  The consumer of the data
    ///     is the <paramref name="onReceive"/> delegate in the subscription.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown if the following param are empty:
    ///     <list type="bullet">
    ///         <item><paramref name="id"/></item>
    ///         <item><paramref name="name"/></item>
    ///     </list>
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the following are null:
    ///     <list type="bullet">
    ///         <item><paramref name="reactable"/></item>
    ///         <item><paramref name="name"/></item>
    ///         <item><paramref name="onReceive"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateOneWayReceive<TIn>(
        this IPushReactable<TIn> reactable,
        Guid id,
        string name,
        Action<TIn> onReceive)
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(onReceive);

        name = name.Trim();

        var projOpenedSub = ISubscriptionBuilder.Create()
            .WithId(id)
            .WithName(name)
            .BuildOneWayReceive(onReceive);

        return reactable.Subscribe(projOpenedSub);
    }

    /// <summary>
    /// Creates a one-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="onReceive">The action to execute when receiving a notification.</param>
    /// <param name="callerMemberName">The name of the caller.</param>
    /// <param name="callerFilePath">The file path of the caller.</param>
    /// <typeparam name="TIn">The type of data coming from the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The direction of travel that the data takes is from the source.  The consumer of the data
    ///     is the <paramref name="onReceive"/> delegate in the subscription.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="id"/> param is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the following are null:
    ///     <list type="bullet">
    ///         <item><paramref name="reactable"/></item>
    ///         <item><paramref name="onReceive"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateOneWayReceive<TIn>(
        this IPushReactable<TIn> reactable,
        Guid id,
        Action<TIn> onReceive,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(onReceive);
        callerMemberName = string.IsNullOrEmpty(callerMemberName) ? string.Empty : callerMemberName;
        callerFilePath = string.IsNullOrEmpty(callerFilePath) ? string.Empty : callerFilePath;

        callerMemberName = callerMemberName.Trim();
        callerFilePath = callerFilePath.Trim();

        callerMemberName = callerMemberName.Contains('.')
            ? callerMemberName.Split('.')[^1]
            : callerMemberName;

        var className = Path.GetFileNameWithoutExtension(callerFilePath);
        var callerName = $"{className}.{callerMemberName}";
        var subName = $"{callerName}() - {id}";

        var projOpenedSub = ISubscriptionBuilder.Create()
            .WithId(id)
            .WithName(subName)
            .BuildOneWayReceive(onReceive);

        return reactable.Subscribe(projOpenedSub);
    }
}
