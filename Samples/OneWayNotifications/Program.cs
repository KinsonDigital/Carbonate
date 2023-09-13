// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedVariable
// ReSharper disable ConvertClosureToMethodGroup

using Carbonate.Fluent;
using Carbonate.OneWay;

// var messenger = new PushReactable<string>(); // Create the messenger object to push notifications
//
// var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event
//
// // Subscribe to the event to receive messages
// IDisposable unsubscriber = messenger.Subscribe(new ReceiveReactor<string>(
//     eventId: msgEventId,
//     name: "my-subscription",
//     onReceiveData: (msg) => Console.WriteLine(msg),
//     onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
//     onError: (ex) => Console.WriteLine($"Error: {ex.Message}")));
//
// messenger.Push("hello world!", msgEventId); // Will invoke all onReceive 'Actions'
// messenger.Unsubscribe(msgEventId); // Will invoke all onUnsubscribe 'Actions'
//
// messenger.Dispose(); // Will also invoke all onUnsubscribe 'Actions'
//
// // NOTE: Throwing an exception in the 'onReceive' action will invoke the 'onError' action

////////////

var subscriptionMessenger = new PushReactable<string>();
var subscriptionEventId = Guid.NewGuid();

var subscriptionBuilder = ISubscriptionBuilder.Create();

var subscription = subscriptionBuilder
    .WithId(subscriptionEventId)
    .WithName(nameof(subscriptionBuilder))
    .WhenUnsubscribing(() => Console.WriteLine("Unsubscribed from notifications!"))
    .WithError(ex => Console.WriteLine($"Error: {ex.Message}"))
    .BuildOneWayReceive<string>(msg => Console.WriteLine(msg));

IDisposable subscriptionUnsubscriber = subscriptionMessenger.Subscribe(subscription);

subscriptionMessenger.Push("hello world", subscriptionEventId);
subscriptionMessenger.Unsubscribe(subscriptionEventId);

////////////////

// var pushBuilderEventId = Guid.NewGuid();
//
// var pushReactableBuilder = IReactableBuilder.Create();
//
// (IDisposable pushBuilderUnsubscriber, IPushReactable<string> pushReactable) = pushReactableBuilder
//     .WithId(pushBuilderEventId)
//     .WithName(nameof(pushReactableBuilder))
//     .WithError(ex => Console.WriteLine($"Error: {ex.Message}"))
//     .BuildUniPush<string>(msg => Console.WriteLine(msg));
//
// pushReactable.Push("This was created by the push reactable factory fluent API", pushBuilderEventId);
// pushReactable.Unsubscribe(pushBuilderEventId);
//
// Console.ReadLine();
