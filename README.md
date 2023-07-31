<div align="center">

![logo](https://raw.githubusercontent.com/KinsonDigital/Carbonate/preview/Images/carbonate-logo-light-mode.png#gh-light-mode-only)
![logo](https://raw.githubusercontent.com/KinsonDigital/Carbonate/preview/Images/carbonate-logo-dark-mode.png#gh-dark-mode-only)
</div>

<h1 style="border:0;font-weight:bold" align="center">Carbonate</h1>

<div align="center">

[![Build PR Status Check](https://img.shields.io/github/actions/workflow/status/KinsonDigital/Carbonate/build-status-check.yml?label=%E2%9A%99%EF%B8%8FBuild)](https://github.com/KinsonDigital/Carbonate/actions/workflows/build-status-check.yml)
[![Test PR Status Check](https://img.shields.io/github/actions/workflow/status/KinsonDigital/Carbonate/test-status-check.yml?label=%F0%9F%A7%AATests)](https://github.com/KinsonDigital/Carbonate/actions/workflows/test-status-check.yml)

[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=KinsonDigital_Carbonate&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=KinsonDigital_Carbonate)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=KinsonDigital_Carbonate&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=KinsonDigital_Carbonate)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=KinsonDigital_Carbonate&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=KinsonDigital_Carbonate)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=KinsonDigital_Carbonate&metric=bugs)](https://sonarcloud.io/summary/new_code?id=KinsonDigital_Carbonate)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=KinsonDigital_Carbonate&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=KinsonDigital_Carbonate)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=KinsonDigital_Carbonate&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=KinsonDigital_Carbonate)

[![Code Coverage](https://img.shields.io/codecov/c/github/KinsonDigital/Carbonate/preview?label=Code%20Coverage&logo=CodeCov&style=flat)](https://app.codecov.io/gh/KinsonDigital/Carbonate)

[![Latest NuGet Release](https://img.shields.io/nuget/vpre/kinsondigital.Carbonate?label=Latest%20Release&logo=nuget)](https://www.nuget.org/packages/KinsonDigital.Carbonate)
[![Nuget Downloads](https://img.shields.io/nuget/dt/KinsonDigital.Carbonate?color=0094FF&label=nuget%20downloads&logo=nuget)](https://www.nuget.org/stats/packages/KinsonDigital.Carbonate?groupby=Version)

[![Good First Issues](https://img.shields.io/github/issues/kinsondigital/Carbonate/good%20first%20issue?color=7057ff&label=Good%20First%20Issues)](https://github.com/KinsonDigital/Carbonate/issues?q=is%3Aissue+is%3Aopen+label%3A%22good+first+issue%22)
[![Discord](https://img.shields.io/discord/481597721199902720?color=%23575CCB&label=chat%20on%20discord&logo=discord&logoColor=white)](https://discord.gg/qewu6fNgv7)
</div>
<h2 style="font-weight:bold;border:0" align="center" >!! NOTICE !!</h2>

This library is still under development and is not at v1.0.0 yet!!  However, all of the major features are available, so we encourage you to use the library and provide feedback.  That is what open source is all about. 🥳

<h2 style="font-weight:bold;border:0" align="center">📖 About Carbonate 📖</h2>

**Carbonate** is a messaging library that uses the observable pattern.  It adds the ability to easily and reliably push and pull messages to different parts or systems of an application.  This helps promote decoupling between different parts of your application as well as increasing your application's testability.

For a real-world example, check out the [Velaptor](https://github.com/KinsonDigital/Velaptor) code base which is an open source 2D game development framework.  This library has been vital for decoupling the different sub-systems and making Velaptor testable.

Go [here](https://refactoring.guru/design-patterns/observer) for a great explanation of the observer pattern.  There are many tutorials and examples of this pattern all over the web.  It is well documented, well known, and a highly used programming pattern.

> **Note** Click [here](https://github.com/KinsonDigital/Carbonate/tree/preview/Samples) to view all of the sample projects.

<h2 style="font-weight:bold;border:0" align="center">✨ Features & Benefits ✨</h2>

**Features:**
- Sending notifications of events with no data
- Sending notifications of events with data
- Sending notifications of events with data that also returns data
- Interfaces and abstractions are provided for custom implementations and to accommodate testing

**Benefits:**
- Increases decoupling
- Increases testability
- Ability to send data and events without the need to change the public API
- Promotes the [Open/Closed Principle](https://www.tutorialsteacher.com/csharp/open-closed-principle)

<h2 style="font-weight:bold;border:0" align="center">💡 Examples 💡</h2>

Below are some examples to demonstrate some basic uses of ***Carbonate***.  Use cases will depend on your application and your needs.  ***Carbonate*** is very flexible but how you use it depends on your needs.

<h3 style="font-weight:bold;color: #00BBC6">Non-directional push notifications</h3>

To send a _**non-directional**_ push notification of something that has occurred, you can use the `PushReactable` type. The subscriber will use the `ReceiveReactor` type for subscriptions. The term _**Non-directional**_ means that no data is being pushed out with the notification or returned from the notification.  This is great for sending a notification that an event has occurred when no data is needed.

```cs
var messenger = new PushReactable(); // Create the messenger object to push notifications
var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
IDisposable unsubscriber = messenger.Subscribe(new ReceiveReactor(
    eventId: msgEventId,
    name: "my-subscription",
    onReceive: () => Console.WriteLine("Received a message!"),
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
));

messenger.Push(msgEventId); // Will invoke all onReceive 'Actions'
unsubscriber.Dispose(); // Will only unsubscribe from this subscription
```

Each notification sent out has a unique id.  Any subscribers to that notification must use the same unique ID to receive that notification.  This is to make sure that the notification is unique and that no logic is required with the other subscriptions to make sure that the notification was meant for them.

> **💡TIP💡**  
> If you want to only receive a single notification, just unsubscribe from further notifications by calling the `Dispose()`
> method on the `IDisposable` object returned by the `PushReactable` object. All reactable objects return this object for unsubscribing at a later time.
> ```cs
> new ReceiveReactor(
>     eventId: msgEventId,
>     onReceive: () =>
>     {
>        // Processing other required logic here . . .
>        unsubscriber.Dispose(); // Will unsubscribe itself when receiving the notification
>     });
> ```

> **Some notes about exceptions and unsubscribing**
> - Throwing an exception in the 'onReceive' action implementation will invoke the 'onError' action for _**ALL**_ subscriptions.
> - Invoking `messenger.Dispose()` will invoke the `onUnsubscribe` action for _**ALL**_ subscriptions.
> - You can unsubscribe from a single subscription by calling the `Dispose()` method on the `IDisposable` object returned by the reactable's `Subscribe()` method.


<h3 style="font-weight:bold;color: #00BBC6">Uni-directional push notifications</h3>

To send data with a push notification in one direction, you can use the `PushReactable<T>` type.  The subscriber will use the `ReceiveReactor<T>` type for subscriptions.  This is set up and used just like the previous example. The term _**one-directional**_ means that data travels in one direction which is out with the push notification.

> **Note** The _**ONLY**_ difference with this example is that your sending some data _**WITH**_ your notification.  
> The generic parameter `T` is the type of data you are sending out.
> All other behaviors are the same.

```cs
var messenger = new PushReactable<string>(); // Create the messenger object to push notifications
var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
IDisposable unsubscriber = messenger.Subscribe(new ReceiveReactor<string>(
    eventId: msgEventId,
    name: "my-subscription",
    onReceiveData: (msg) => Console.WriteLine(msg),
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
));

messenger.Push("hello world!", msgEventId); // Will invoke all onReceive 'Actions'
messenger.Unsubscribe(msgEventId); // Will invoke all onUnsubscribe 'Actions'
```


<h3 style="font-weight:bold;color: #00BBC6">Bi-directional push notifications</h3>

To send a _**bi-directional**_ push notification that sends data out and gets data back, you can use the `PullReactable<T, T>` type.  The subscriber will use the `RespondReactor<T, T>` to subscribe to notifications.  This is used when you want to send a push notification with data that the receiver needs, and then the receiver returns data to the caller that pushed the notification.

```cs
var favoriteGetter = new PullReactable<string, string>();
var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

var unsubscriber = favoriteGetter.Subscribe(new RespondReactor<string, string>(
    respondId: msgEventId,
    name: "adder",
    onRespondData: (data) => data switch
        {
            "prog-lang" => "C#",
            "food" => "scotch eggs",
            "past-time" => "game development",
            "music" => "hard rock/metal",
        },
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
));

Console.WriteLine($"Favorite Language: {favoriteGetter.Pull("prog-lang", msgEventId)}");
Console.WriteLine($"Favorite Food: {favoriteGetter.Pull("food", msgEventId)}");
Console.WriteLine($"Favorite Past Time: {favoriteGetter.Pull("past-time", msgEventId)}");
Console.WriteLine($"Favorite Music: {favoriteGetter.Pull("music", msgEventId)}");
```

> **Note** The difference between _**bi-directional**_ and _**uni-directional**_ notifications is that _**bi-directional**_ notifications send data out and expected data to be returned.

> **💡TIP💡** Most of the time, the `PushReactable`, `PushReactable<T>`, and `PullReactable<string, string>` types will suit your needs.  But if you have any requirements that these cannot provide, you can always create implementations of the interfaces provided.

<h2 style="font-weight:bold;" align="center">🙏🏼 Contributing 🙏🏼</h2>

Interested in contributing? If so, click [here](https://github.com/KinsonDigital/.github/blob/master/docs/CONTRIBUTING.md) to learn how to contribute your time or [here](https://github.com/sponsors/KinsonDigital) if you are interested in contributing your funds via a one-time or recurring donation.

<h2 style="font-weight:bold;border:0" align="center">🔧 Maintainers 🔧</h2>

[![twitter-logo](https://raw.githubusercontent.com/KinsonDigital/.github/master/Images/twitter-logo-16x16.svg)Calvin Wilkinson](https://twitter.com/KDCoder) (KinsonDigital GitHub Organization - Owner)

[![twitter-logo](https://raw.githubusercontent.com/KinsonDigital/.github/master/Images/twitter-logo-16x16.svg)Kristen Wilkinson](https://twitter.com/kswilky) (KinsonDigital GitHub Organization - Documentation Maintainer & Tester)


<h2 style="font-weight:bold;border:0" align="center">🚔 Licensing and Governance 🚔</h2>

<div align="center">


[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg?style=flat)](https://github.com/KinsonDigital/.github/blob/master/docs/code_of_conduct.md)
[![GitHub](https://img.shields.io/github/license/kinsondigital/Carbonate)](https://github.com/KinsonDigital/Carbonate/blob/preview/v1.0.0/LICENSE.md)

<div align= "left">

This software is distributed under the very permissive MIT license and all dependencies are distributed under MIT-compatible licenses.
This project has adopted the code of conduct defined by the **Contributor Covenant** to clarify expected behavior in our community.
