// <copyright file="Reactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Services;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
public sealed class Reactable : IReactable
{
    private readonly List<IReactor> reactors = new ();
    private readonly ISerializerService serializerService;
    private bool isDisposed;
    private bool notificationsEnded;

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactable"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API for users.")]
    public Reactable() => this.serializerService = new JsonSerializerService();

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactable"/> class.
    /// </summary>
    /// <param name="serializerService">The serializer used to serialize the messages.</param>
    public Reactable(ISerializerService serializerService) => this.serializerService = serializerService;

    /// <inheritdoc/>
    public ReadOnlyCollection<IReactor> Reactors => new (this.reactors);

    /// <inheritdoc/>
    public ReadOnlyCollection<Guid> EventIds => this.reactors
        .Select(r => r.EventId)
        .Distinct()
        .ToReadOnlyCollection();

    /// <inheritdoc/>
    public IDisposable Subscribe(IReactor reactor)
    {
        if (reactor is null)
        {
            throw new ArgumentNullException(nameof(reactor), "The parameter must not be null.");
        }

        this.reactors.Add(reactor);

        return new ReactorUnsubscriber(this.reactors, reactor);
    }

    /// <inheritdoc/>
    public void Push(Guid eventId) => SendNotifications(null, eventId);

    /// <inheritdoc/>
    public void PushData<T>(in T data, Guid eventId)
    {
        try
        {
            var jsonData = this.serializerService.Serialize(data);

            var message = new JsonMessage(this.serializerService, jsonData);

            SendNotifications(message, eventId);
        }
        catch (JsonException e)
        {
            SendError(e, eventId);
        }
    }

    /// <inheritdoc/>
    public void PushMessage(in IMessage message, Guid eventId) => SendNotifications(message, eventId);

    /// <inheritdoc/>
    public void Unsubscribe(Guid eventId)
    {
        if (this.notificationsEnded)
        {
            return;
        }

        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnNext() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.reactors.Count - 1
                ? this.reactors.Count - 1
                : i;

            if (this.reactors[i].EventId != eventId)
            {
                continue;
            }

            var beforeTotal = this.reactors.Count;

            this.reactors[i].OnComplete();

            var nothingRemoved = Math.Abs(beforeTotal - this.reactors.Count) <= 0;

            // Make sure that the OnComplete implementation did not remove
            // the reactor before attempting to remove it
            if (nothingRemoved)
            {
                this.reactors.Remove(this.reactors[i]);
            }
        }

        this.notificationsEnded = this.reactors.All(r => r.Unsubscribed);
    }

    /// <inheritdoc/>
    public void UnsubscribeAll()
    {
        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnNext() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.reactors.Count - 1
                ? this.reactors.Count - 1
                : i;

            this.reactors[i].OnComplete();
        }

        this.reactors.Clear();
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => Dispose(true);

    /// <summary>
    /// Sends a notification to all of the <see cref="IReactor"/>s that match the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="eventId">The ID of the event.</param>
    private void SendNotifications(IMessage? message, Guid eventId)
    {
        /* Work from the end to the beginning of the list
         * just in case the reactable is disposed(removed)
         * in the OnNext() method.
         */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnNext() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.reactors.Count - 1
                ? this.reactors.Count - 1
                : i;

            if (this.reactors[i].EventId != eventId)
            {
                continue;
            }

            if (message is null)
            {
                this.reactors[i].OnNext();
            }
            else
            {
                this.reactors[i].OnNext(message);
            }
        }
    }

    /// <summary>
    /// Sends an error to all of the subscribers that matches the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    /// <param name="eventId">The ID of the event where the notification will be pushed.</param>
    private void SendError(Exception exception, Guid eventId)
    {
        /* Work from the end to the beginning of the list
         * just in case the reactable is disposed(removed)
         * in the OnNext() method.
         */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnNext() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > this.reactors.Count - 1
                ? this.reactors.Count - 1
                : i;

            if (this.reactors[i].EventId != eventId)
            {
                continue;
            }

            this.reactors[i].OnError(exception);
        }
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="disposing">Disposes managed resources when <c>true</c>.</param>
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            UnsubscribeAll();
        }

        this.isDisposed = true;
    }
}
