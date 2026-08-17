// <copyright file="MessageExtensions.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Messaging;

using Carbonate.NonDirectional;
using Carbonate.OneWay;
using Carbonate.TwoWay;

/// <summary>
/// Strongly-typed <c>Push</c>/<c>Pull</c>/<c>Subscribe</c> extensions that bind a declared
/// message to the correct Carbonate v1 reactable at compile time.
/// Because the message type parameter appears in both the reactable and the data,
/// pairing the wrong message with the wrong bus or payload is a compile error.
/// </summary>
public static class MessageExtensions
{
    // ---------- Non-directional ----------

    /// <summary>Pushes a non-directional <paramref name="message"/> to all subscribers.</summary>
    public static void Push(this IPushReactable reactable, Event message)
        => reactable.Push(message.Id);

    /// <summary>Subscribes <paramref name="handler"/> to a non-directional <paramref name="message"/>.</summary>
    public static IDisposable Subscribe(this IPushReactable reactable, Event message, Action handler)
        => reactable.Subscribe(new ReceiveSubscription(message.Id, handler, message.Name));

    // ---------- One-way push ----------

    /// <summary>Pushes <paramref name="data"/> for the one-way <paramref name="message"/>.</summary>
    public static void Push<T>(this IPushReactable<T> reactable, Event<T> message, in T data)
        => reactable.Push(message.Id, data);

    /// <summary>Subscribes <paramref name="handler"/> to the one-way <paramref name="message"/>.</summary>
    public static IDisposable Subscribe<T>(this IPushReactable<T> reactable, Event<T> message, Action<T> handler)
        => reactable.Subscribe(new ReceiveSubscription<T>(message.Id, handler, message.Name));

    // ---------- One-way pull ----------

    /// <summary>Pulls a response for the one-way <paramref name="message"/>.</summary>
    public static TOut? Pull<TOut>(this IPullReactable<TOut> reactable, Request<TOut> message)
        => reactable.Pull(message.Id);

    /// <summary>Subscribes a responder to the one-way <paramref name="message"/>.</summary>
    public static IDisposable Respond<TOut>(this IPullReactable<TOut> reactable, Request<TOut> message, Func<TOut> responder)
        => reactable.Subscribe(new RespondSubscription<TOut>(message.Id, responder, message.Name));

    // ---------- Two-way ----------

    /// <summary>Pushes <paramref name="data"/> for the two-way <paramref name="message"/> and returns the response.</summary>
    public static TOut? PushPull<TIn, TOut>(this IPushPullReactable<TIn, TOut> reactable, Request<TIn, TOut> message, in TIn data)
        => reactable.PushPull(message.Id, data);

    /// <summary>Subscribes a responder to the two-way <paramref name="message"/>.</summary>
    public static IDisposable Respond<TIn, TOut>(
        this IPushPullReactable<TIn, TOut> reactable,
        Request<TIn, TOut> message,
        Func<TIn, TOut> responder)
        => reactable.Subscribe(new ReceiveRespondSubscription<TIn, TOut>(message.Id, responder, message.Name));
}
