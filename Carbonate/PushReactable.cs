// <copyright file="PushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Services;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
public sealed class PushReactable : ReactableBase<IReceiveReactor>, IPushable
{
    private readonly ISerializerService serializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PushReactable"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API for users.")]
    public PushReactable() => this.serializerService = new JsonSerializerService();

    /// <summary>
    /// Initializes a new instance of the <see cref="PushReactable"/> class.
    /// </summary>
    /// <param name="serializerService">The serializer used to serialize the messages.</param>
    public PushReactable(ISerializerService serializerService) =>
        this.serializerService = serializerService ??
            throw new ArgumentNullException(nameof(serializerService), "The parameter must not be null.");

    /// <inheritdoc/>
    public void Push(Guid eventId) => SendNotifications(null, eventId);

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public void PushData<T>(in T data, Guid eventId)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable), $"{nameof(PushReactable)} disposed.");
        }

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
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public void PushMessage(in IMessage message, Guid eventId) => SendNotifications(message, eventId);

    /// <summary>
    /// Sends a notification to all of the <see cref="IReactor"/>s that match the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="eventId">The ID of the event.</param>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    private void SendNotifications(IMessage? message, Guid eventId)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable), $"{nameof(PushReactable)} disposed.");
        }

        /* Work from the end to the beginning of the list
         * just in case the reactable is disposed(removed)
         * in the OnReceive() method.
         */
        for (var i = Reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > Reactors.Count - 1
                ? Reactors.Count - 1
                : i;

            if (Reactors[i].Id != eventId)
            {
                continue;
            }

            if (message is null)
            {
                Reactors[i].OnReceive();
            }
            else
            {
                Reactors[i].OnReceive(message);
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
         * in the OnReceive() method.
         */
        for (var i = Reactors.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > Reactors.Count - 1
                ? Reactors.Count - 1
                : i;

            if (Reactors[i].Id != eventId)
            {
                continue;
            }

            Reactors[i].OnError(exception);
        }
    }
}
