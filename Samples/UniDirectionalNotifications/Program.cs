// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedVariable
// ReSharper disable ConvertClosureToMethodGroup
using Carbonate.UniDirectional;

var messenger = new PushReactable<string>(); // Create the messenger object to push notifications

var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
IDisposable unsubscriber = messenger.Subscribe(new ReceiveReactor<string>(
    eventId: msgEventId,
    name: "my-subscription",
    onReceiveData: (msg) => Console.WriteLine(msg),
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")));

messenger.Push("hello world!", msgEventId); // Will invoke all onReceive 'Actions'
messenger.Unsubscribe(msgEventId); // Will invoke all onUnsubscribe 'Actions'

// messenger.Dispose(); // Will also invoke all onUnsubscribe 'Actions'

// NOTE: Throwing an exception in the 'onReceive' action will invoke the 'onError' action
