// <copyright file="IReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.ObjectModel;

/// <summary>
/// Defines a provider for push-based notification.
/// </summary>
public interface IReactable : IDisposable
{
    /// <summary>
    /// Gets the list of reactors that are subscribed to this <see cref="Reactable"/>.
    /// </summary>
    ReadOnlyCollection<IReactor> Reactors { get; }

    /// <summary>
    /// Gets the list of event IDs.
    /// </summary>
    ReadOnlyCollection<Guid> EventIds { get; }

    /// <summary>
    /// Notifies the provider that an reactor is to receive notifications.
    /// </summary>
    /// <param name="reactor">The object that is to receive notifications.</param>
    /// <returns>
    ///     A reference to an interface that allows reactors to stop receiving
    ///     notifications before the provider has finished sending them.
    /// </returns>
    IDisposable Subscribe(IReactor reactor);

    /// <summary>
    /// Pushes a single notification with the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data to send with the push notification.</param>
    /// <param name="eventId">The ID of the event to push the notification to.</param>
    /// <typeparam name="T">The type of data to push.</typeparam>
    void Push<T>(in T data, Guid eventId);

    /// <summary>
    /// Pushes a single notification using the given <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message that contains the data to push.</param>
    /// <param name="eventId">The ID of the event to push the notifications to.</param>
    public void Push(in IMessage message, Guid eventId);

    /// <summary>
    /// Unsubscribes notifications to all <see cref="Reactor"/>s that match the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="eventId">The ID of the event to end.</param>
    /// <remarks>
    ///     Will not invoke the <see cref="IReactor"/>.<see cref="IReactor.OnComplete"/> more than once.
    /// </remarks>
    void Unsubscribe(Guid eventId);

    /// <summary>
    /// Unsubscribes all of the currently subscribed reactors.
    /// </summary>
    void UnsubscribeAll();
}
