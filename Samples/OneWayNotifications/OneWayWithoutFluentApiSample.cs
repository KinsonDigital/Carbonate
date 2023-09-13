// <copyright file="OneWayWithoutFluentApiSample.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation
#pragma warning disable SA1512
namespace OneWayNotifications;

using System.Diagnostics.CodeAnalysis;
using Carbonate.OneWay;

/// <summary>
/// Shows how to perform one way notifications using a <see cref="PushReactable{TIn}"/> object without using the fluent API.
/// </summary>
[SuppressMessage("csharpsquid", "S125", Justification = "Needed for sample.")]
public class OneWayWithoutFluentApiSample : Sample
{
    /// <inheritdoc/>
    public override void Run()
    {
        var description = "This sample shows how to create a 'one way' reactable object without using the fluent API.";
        description += "\nThe is the lowest level and most verbose approach to creating a reactable and";
        description += "\nfor subscribing to push notifications from the reactable.";
        description += "\n\nThis setup is useful if you want to send data with a push notification so the";
        description += "\nsubscriber can use the data.";

        PrintDescription(description);

        var dataSender = new PushReactable<string>(); // Create the messenger object to push notifications

        var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

        // Subscribe to the event to receive messages
        var unsubscriber = dataSender.Subscribe(new ReceiveSubscription<string>(
            id: msgEventId,
            onReceive: Console.WriteLine,
            name: "my-subscription",
            onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
            onError: (ex) => Console.WriteLine($"Error: {ex.Message}")));

        dataSender.Push("hello world!", msgEventId); // Will invoke all onReceive 'Actions'
        unsubscriber.Dispose(); // This will only unsubscribe the single subscription created above

        // This will unsubscribe all subscriptions for the given event ID
        // that are subscribed to this reactable object.
        // dataSender.Unsubscribe(msgEventId);

        // dataSender.Dispose(); // Will also invoke all onUnsubscribe 'Actions'

        // NOTE: Throwing an exception in the 'onReceive' action will invoke the 'onError' action
    }
}
