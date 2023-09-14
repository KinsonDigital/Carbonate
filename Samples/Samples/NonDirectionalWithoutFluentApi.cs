// <copyright file="NonDirectionalWithoutFluentApi.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
#pragma warning disable SA1512
namespace Samples;

using System.Diagnostics.CodeAnalysis;
using Carbonate.NonDirectional;

/// <summary>
/// Shows how to perform notifications with no data going out or coming back using
/// a <see cref="PushReactable"/> object without using the fluent API.
/// </summary>
[SuppressMessage("csharpsquid", "S125", Justification = "Needed for sample.")]
public class NonDirectionalWithoutFluentApi : Sample
{
    /// <inheritdoc/>
    public override void Run()
    {
        var description = "This sample shows how to create a reactable object without using the fluent API.";
        description += "\nThe is the lowest level and most verbose approach to creating a reactable and";
        description += "\nfor subscribing to push notifications from the reactable.";
        description += "\n\nThis setup is useful if you just want to send a push notification";
        description += "\nwithout any data and without getting date back.";
        description += "\nThink about this as simply send an event that something happened.";

        PrintDescription(description);

        var eventSender = new PushReactable(); // Create the messenger object to push notifications

        var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

        // Subscribe to the event to receive messages
        var unsubscriber = eventSender.Subscribe(new ReceiveSubscription(
            id: msgEventId,
            name: "my-subscription",
            onReceive: () => Console.WriteLine("Received a message!"),
            onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
            onError: (ex) => Console.WriteLine($"Error: {ex.Message}")));

        eventSender.Push(msgEventId); // Will invoke all onReceive 'Actions'
        unsubscriber.Dispose(); // Will only unsubscribe from this subscription

        // This will unsubscribe all subscriptions for the given event ID
        // that are subscribed to this reactable object.
        // eventSender.Unsubscribe(msgEventId);

        // eventSender.Dispose(); // Will also invoke all onUnsubscribe 'Actions'

        // NOTE: Throwing an exception in the 'onReceive' action will invoke the 'onError' action
    }
}
