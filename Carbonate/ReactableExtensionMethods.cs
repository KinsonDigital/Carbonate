// <copyright file="ReactableExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Runtime.CompilerServices;
using Fluent;
using NonDirectional;
using OneWay;
using TwoWay;

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
    /// <param name="onReceive">The delegate to execute when receiving a notification.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     No data travels to or from the subscription.
    ///     <br/>
    ///     Note:  The <paramref name="name"/> parameter is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
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
    public static IDisposable CreateNonReceiveOrRespond(
        this IPushReactable reactable,
        Guid id,
        string name,
        Action onReceive,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(onReceive);

        name = name.Trim();

        var subscription = (onSubscribe: onUnsubscribe, onError) switch
        {
            (null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .BuildNonReceiveOrRespond(onReceive),
            (null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WithError(onError)
                .BuildNonReceiveOrRespond(onReceive),
            (not null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .BuildNonReceiveOrRespond(onReceive),
            (not null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .WithError(onError)
                .BuildNonReceiveOrRespond(onReceive)
        };

        return reactable.Subscribe(subscription);
    }

    /// <summary>
    /// Creates a non-directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="onReceive">The delegate to execute when receiving a notification.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
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
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null,
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

        return reactable.CreateNonReceiveOrRespond(id, subName, onReceive, onUnsubscribe, onError);
    }

    /// <summary>
    /// Creates a one-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="name">The name of the subscription.</param>
    /// <param name="onReceive">The delegate to execute to receive data from the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <typeparam name="TIn">The type of data coming from the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The data travels from the source to the subscription.
    ///     <br/>
    ///     Note:  The <paramref name="name"/> parameter is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
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
        Action<TIn> onReceive,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(onReceive);

        name = name.Trim();

        var subscription = (onSubscribe: onUnsubscribe, onError) switch
        {
            (null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .BuildOneWayReceive(onReceive),
            (null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WithError(onError)
                .BuildOneWayReceive(onReceive),
            (not null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .BuildOneWayReceive(onReceive),
            (not null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .WithError(onError)
                .BuildOneWayReceive(onReceive)
        };

        return reactable.Subscribe(subscription);
    }

    /// <summary>
    /// Creates a one-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceive"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="onReceive">The delegate to execute to receive data from the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <param name="callerMemberName">The name of the caller.</param>
    /// <param name="callerFilePath">The file path of the caller.</param>
    /// <typeparam name="TIn">The type of data coming from the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The data travels from the source to the subscription.
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
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null,
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

        return reactable.CreateOneWayReceive(id, subName, onReceive, onUnsubscribe, onError);
    }

    /// <summary>
    /// Creates a one-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onRespond"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="name">The name of the subscription.</param>
    /// <param name="onRespond">The delegate to execute to respond with data to the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <typeparam name="TOut">The type of data going back to the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The data travels from the subscription and back to the source.
    ///     <br/>
    ///     Note:  The <paramref name="name"/> parameter is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
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
    ///         <item><paramref name="onRespond"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateOneWayRespond<TOut>(
        this IPullReactable<TOut> reactable,
        Guid id,
        string name,
        Func<TOut> onRespond,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(onRespond);

        name = name.Trim();

        var subscription = (onSubscribe: onUnsubscribe, onError) switch
        {
            (null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .BuildOneWayRespond(onRespond),
            (null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WithError(onError)
                .BuildOneWayRespond(onRespond),
            (not null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .BuildOneWayRespond(onRespond),
            (not null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .WithError(onError)
                .BuildOneWayRespond(onRespond)
        };

        return reactable.Subscribe(subscription);
    }

    /// <summary>
    /// Creates a one-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onRespond"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="onRespond">The delegate to execute to respond with data to the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <param name="callerMemberName">The name of the caller.</param>
    /// <param name="callerFilePath">The file path of the caller.</param>
    /// <typeparam name="TOut">The type of data going back to the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The data travels from the subscription and back to the source.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="id"/> param is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the following are null:
    ///     <list type="bullet">
    ///         <item><paramref name="reactable"/></item>
    ///         <item><paramref name="onRespond"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateOneWayRespond<TOut>(
        this IPullReactable<TOut> reactable,
        Guid id,
        Func<TOut> onRespond,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(onRespond);
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

        return reactable.CreateOneWayRespond(id, subName, onRespond, onUnsubscribe, onError);
    }

    /// <summary>
    /// Creates a two-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceiveRespond"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="name">The name of the subscription.</param>
    /// <param name="onReceiveRespond">The delegate to execute to receive data from the source and return data to the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <typeparam name="TIn">The type of data coming from the source.</typeparam>
    /// <typeparam name="TOut">The type of data going back to the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The data travels from the source to the subscription and back to the source.
    ///     <br/>
    ///     Note:  The <paramref name="name"/> parameter is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
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
    ///         <item><paramref name="onReceiveRespond"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateTwoWay<TIn, TOut>(
        this IPushPullReactable<TIn, TOut> reactable,
        Guid id,
        string name,
        Func<TIn, TOut> onReceiveRespond,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(onReceiveRespond);

        name = name.Trim();

        var subscription = (onSubscribe: onUnsubscribe, onError) switch
        {
            (null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .BuildTwoWay(onReceiveRespond),
            (null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WithError(onError)
                .BuildTwoWay(onReceiveRespond),
            (not null, null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .BuildTwoWay(onReceiveRespond),
            (not null, not null) => ISubscriptionBuilder.Create()
                .WithId(id)
                .WithName(name)
                .WhenUnsubscribing(onUnsubscribe)
                .WithError(onError)
                .BuildTwoWay(onReceiveRespond)
        };

        return reactable.Subscribe(subscription);
    }

    /// <summary>
    /// Creates a two-way directional reactable subscription for the given <paramref name="reactable"/>
    /// with the given <paramref name="onReceiveRespond"/> that will be executed every time a notification is pushed from a source.
    /// </summary>
    /// <param name="reactable">The reactable to create a subscription with.</param>
    /// <param name="id">The unique id.</param>
    /// <param name="onReceiveRespond">The delegate to execute to receive data from the source and return data to the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <param name="callerMemberName">The name of the caller.</param>
    /// <param name="callerFilePath">The file path of the caller.</param>
    /// <typeparam name="TIn">The type of data coming from the source.</typeparam>
    /// <typeparam name="TOut">The type of data going back to the source.</typeparam>
    /// <returns>
    ///     A <see cref="IDisposable"/> object that can be used to unsubscribe the subscription.
    /// </returns>
    /// <remarks>
    ///     The data travels from the source to the subscription and back to the source.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="id"/> param is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the following are null:
    ///     <list type="bullet">
    ///         <item><paramref name="reactable"/></item>
    ///         <item><paramref name="onReceiveRespond"/></item>
    ///     </list>
    /// </exception>
    public static IDisposable CreateTwoWay<TIn, TOut>(
        this IPushPullReactable<TIn, TOut> reactable,
        Guid id,
        Func<TIn, TOut> onReceiveRespond,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(reactable);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(onReceiveRespond);
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

        return reactable.CreateTwoWay(id, subName, onReceiveRespond, onUnsubscribe, onError);
    }
}
