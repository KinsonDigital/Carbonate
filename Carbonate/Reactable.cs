// <copyright file="Reactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Services;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
public sealed class Reactable : IReactable
{
    private readonly List<IReactor> reactors = new ();
    private readonly ISerializer serializer;
    private bool isDisposed;
    private bool notificationsEnded;

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactable"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API for users.")]
    public Reactable() => this.serializer = new JsonSerializer();

    /// <summary>
    /// Initializes a new instance of the <see cref="Reactable"/> class.
    /// </summary>
    /// <param name="serializer">The serializer used to serialize the messages.</param>
    public Reactable(ISerializer serializer) => this.serializer = serializer;

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
    public void Push<T>(in T data, Guid eventId)
    {
        var jsonData = this.serializer.Serialize(data);

        var message = new JsonMessage(this.serializer, jsonData);

        SendNotifications(message, eventId);
    }

    /// <inheritdoc/>
    public void Push(in IMessage message, Guid eventId) => SendNotifications(message, eventId);

    /// <inheritdoc/>
    public void Unsubscribe(Guid eventId)
    {
        if (this.notificationsEnded)
        {
            return;
        }

        // ReSharper disable ForCanBeConvertedToForeach
        /* Keep this loop as a for-loop.  Do not convert to for-each.
         * This is due to the Dispose() method possibly being called during
         * iteration of the reactors list which will cause an exception.
        */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            if (this.reactors[i].EventId == eventId)
            {
                this.reactors[i].OnComplete();
            }
        }

        // ReSharper restore ForCanBeConvertedToForeach
        this.notificationsEnded = this.reactors.All(r => r.Unsubscribed);
    }

    /// <inheritdoc/>
    public void UnsubscribeAll() => this.reactors.Clear();

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => Dispose(true);

    private void SendNotifications(IMessage message, Guid eventId)
    {
        /* Work from the end to the beginning of the list
           just in case the reactable is disposed(removed)
           in the OnNext() method.
         */
        for (var i = this.reactors.Count - 1; i >= 0; i--)
        {
            if (this.reactors[i].EventId == eventId)
            {
                this.reactors[i].OnNext(message);
            }
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
            var uniqueIdValues = this.reactors
                .Select(r => r.EventId)
                .Distinct();

            foreach (var id in uniqueIdValues)
            {
                Unsubscribe(id);
            }

            this.reactors.Clear();
        }

        this.isDisposed = true;
    }
}
