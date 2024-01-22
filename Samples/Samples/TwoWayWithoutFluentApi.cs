// <copyright file="TwoWayWithoutFluentApi.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
#pragma warning disable SA1512
namespace Samples;

using System.Diagnostics.CodeAnalysis;
using Carbonate.TwoWay;

/// <summary>
/// Shows how to perform two way notifications using a <see cref="PushPullReactable{TIn, TOut}"/> object without using the fluent API.
/// </summary>
[SuppressMessage("csharpsquid", "S125", Justification = "Needed for sample.")]
public class TwoWayWithoutFluentApi : Sample
{
    /// <inheritdoc/>
    public override void Run()
    {
        var description = "This sample shows how to create a 'two way' reactable object without using the fluent API.";
        description += "\nThe is the lowest level and most verbose approach to creating a reactable and";
        description += "\nfor subscribing to push notifications from the reactable.";
        description += "\n\nThis setup is useful if you want to send and receive data with a push notification";

        PrintDescription(description);

        var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event
        var favoriteRequester = new PushPullReactable<string, string>();

        var unsubscriber = favoriteRequester.Subscribe(new ReceiveRespondSubscription<string, string>(
            id: msgEventId,
            name: "adder",
            onReceiveRespond: data => data switch
            {
                "prog-lang" => "C#",
                "food" => "scotch eggs",
                "past-time" => "game development",
                "music" => "hard rock/metal",
            },
            onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
            onError: ex => Console.WriteLine($"Error: {ex.Message}")));

        // Print the results of the data returned for each push notification
        Console.WriteLine($"Favorite Language: {favoriteRequester.PushPull(msgEventId, "prog-lang")}");
        Console.WriteLine($"Favorite Food: {favoriteRequester.PushPull(msgEventId, "food")}");
        Console.WriteLine($"Favorite Past Time: {favoriteRequester.PushPull(msgEventId, "past-time")}");
        Console.WriteLine($"Favorite Music: {favoriteRequester.PushPull(msgEventId, "music")}");

        unsubscriber.Dispose();

        // This will unsubscribe all subscriptions for the given event ID
        // that are subscribed to this reactable object.
        // dataSender.Unsubscribe(msgEventId);

        // eventSender.Dispose(); // Will also invoke all onUnsubscribe 'Actions'

        // NOTE: Throwing an exception in the 'onReceive' action will invoke the 'onError' action
    }
}
