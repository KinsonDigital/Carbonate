// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using Carbonate.NonDirectional;

var messenger = new PushReactable(); // Create the messenger object to push notifications

var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
IDisposable unsubscriber = messenger.Subscribe(new ReceiveReactor(
    eventId: msgEventId,
    name: "my-subscription",
    onReceive: () => Console.WriteLine("Received a message!"),
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")));

messenger.Push(msgEventId); // Will invoke all onReceive 'Actions'
unsubscriber.Dispose(); // Will only unsubscribe from this subscription

// messenger.Dispose(); // Will also invoke all onUnsubscribe 'Actions'

// NOTE: Throwing an exception in the 'onReceive' action will invoke the 'onError' action
