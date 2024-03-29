<div align="center">

![logo](https://raw.githubusercontent.com/KinsonDigital/Carbonate/preview/Images/carbonate-logo-light-mode.svg#gh-light-mode-only)
![logo](https://raw.githubusercontent.com/KinsonDigital/Carbonate/preview/Images/carbonate-logo-dark-mode.svg#gh-dark-mode-only)
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

**Carbonate** is a messaging library built on the observable pattern, empowering seamless and dependable push-and-pull message handling across various parts or systems within an application. This fosters decoupling among different components, enhancing your application's overall testability as well as separating cross-cutting concerns.

You can choose if you want to send out a push notification with or without data or if you want to poll for a notification with or without data.  These result in data only flowing in one direction.

You can also choose to push data out and receive data back in a single notification.

For a real-world example, check out the [Velaptor](https://github.com/KinsonDigital/Velaptor) code base which is an open-source 2D game development framework.  This library has been vital for decoupling the different sub-systems and increasing its testability.

Go [here](https://refactoring.guru/design-patterns/observer) for information on the observable pattern. This design pattern has been extensively covered in various tutorials and examples across the web, making it well-documented, widely recognized, and a highly popular programming pattern.

> [!Note]
> Click [here](https://github.com/KinsonDigital/Carbonate/tree/preview/Samples/Samples) to view all of the sample projects.

<h2 style="font-weight:bold;border:0" align="center">✨ Features & Benefits ✨</h2>

**Features:**
- Send push notifications with no data
- Send push notifications with data only going out
- Send push notifications with data only being returned
- Send push notifications with data going out and and being returned
- Interfaces and abstractions are provided for custom implementations and to provide testability


**Benefits:**
- Increases decoupling
- Increases testability
- Works well with dependency injection
- Sends data and events without needing to change the public API of your library/project
- Promotes the [Open/Closed Principle](https://www.tutorialsteacher.com/csharp/open-closed-principle)

<h2 style="font-weight:bold;border:0" align="center">💡 Examples 💡</h2>

Below are some examples to demonstrate some basic uses of ***Carbonate***.  This library is very flexible but how you use it depends on the needs of your application.

<h3 style="font-weight:bold;color: #00BBC6">Non-directional push notifications</h3>

To send a _**non-directional**_ push notification, you can use the `PushReactable` class. You subscribe using the `Subscribe()` method by sending in the subscription object. The term _**non-directional**_ means that no data is being sent out or returned from the notification call stack.  This is great for sending a notification that an event has occurred when no data is needed.

Every notification sent out contains a unique ID, which subscribers must use to receive the intended notification, ensuring its exclusivity and eliminating the need for additional logic to filter out each notification going out.

<details closed><summary>Subscription Example</summary>

```cs
var messenger = new PushReactable(); // Create the messenger object to push notifications
var subId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
var subscription = new ReceiveSubscription(
    id: subId,
    onReceive: () => Console.WriteLine("Received a message!"),
    name: "my-subscription",
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
);

IDisposable unsubscriber = messenger.Subscribe(subscription);

messenger.Push(subId); // Will invoke all onReceive 'Actions' subscribed to this reactable
unsubscriber.Dispose(); // Will only unsubscribe from this subscription
```
</details>

<br/>

<details closed><summary>How To Unsubscribe Example</summary>

```cs
var mySubscription = new ReceiveSubscription(
    id: subId,
    name: "my-subscription",
    onReceive: () => { Console.WriteLine("Received notification!"); }
    onUnsubscribe: () =>
    {
       unsubscriber.Dispose(); // Will unsubscribe from further notifications
    });
```
</details>

<br/>

<details closed><summary>How Not To Unsubscribe Example</summary>

Below is an example of what you _**SHOULD NOT**_ do.
```cs
IDisposable? unsubscriber;
var subId = Guid.NewGuid(); // This is the ID used to identify the event

var badSubscription = new ReceiveSubscription(
    id: subId,
    name: "bad-subscription",
    onReceive: () =>
    {
        // DO NOT DO THIS!!
        unsubscriber.Dispose(); // An exception will be thrown in here
    });
var messenger = new PushReactable();
unsubscriber = messenger.Subscribe(badSubscription);
messenger.Push(subId);
```
</details>

> [!Tip]
> If you want to receive a single notification, unsubscribe from further notifications by calling the `Dispose()`
> method on the `IDisposable` object returned by the _**Reactable**_ object. All reactable objects return an unsubscriber object for unsubscribing at a later time.  The unsubscriber is returned when invoking the `Subscribe()` method.  Unsubscribing can be done anytime except in the notification delegates `onReceive`, `onRespond`, and `onReceiveRespond`.

> [!Tip]
> If an attempt is made to unsubscribe from notifications inside of any of the notification delegates, a `NotificationException` will be
> thrown.  This is an intentional design to prevent the removal of any internal subscriptions during the notification process.
> Of course, you can add a `try...catch` in the notification delegate to swallow the exception, but again this is not recommended.

<h3 style="font-weight:bold;color: #00BBC6">One way push notifications</h3>

To facilitate _**one way**_ data transfer through push notifications, you can employ the `PushReactable<TIn>` or `PullReactable<TOut>` types while subscribers utilize the `ReceiveSubscription<TIn>` or `RespondSubscription<TOut>` types for their subscriptions. Setting up and using this approach follows the same steps as in the previous example. In this context, the term one-directional signifies that data exclusively flows in one direction either out from the source to the subscription delegate or from the subscription delegate to the source.

<details closed><summary>One Way Out Notification Example</summary>

```cs
var messenger = new PushReactable<string>(); // Create the messenger object to push notifications with data
var subId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
IDisposable unsubscriber = messenger.Subscribe(new ReceiveSubscription<string>(
    id: subId,
    onReceive: (msg) => Console.WriteLine(msg),
    name: "my-subscription",
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
));

messenger.Push("hello from source!", subId); // Will invoke all onReceive 'Actions' that have subscribed with 'subId'.
messenger.Unsubscribe(subId); // Will invoke all onUnsubscribe 'Actions' that have subscribed with 'subId'.
```
</details>

<br/>

<details closed><summary>One Way In Notification(Polling) Example</summary>

```cs
var messenger = new PullReactable<string>(); // Create the messenger object to push notifications to receive data
var subId = Guid.NewGuid(); // This is the ID used to identify the event

// Subscribe to the event to receive messages
IDisposable unsubscriber = messenger.Subscribe(new RespondSubscription<string>(
    id: subId,
    onRespond: (msg) => "hello from subscriber!",
    name: "my-subscription",
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
));

var response = messenger.Pull(subId); // Will invoke all onRespond 'Actions' that have subscribed with 'subId'.
Console.WriteLine(response);
messenger.Unsubscribe(subId); // Will invoke all onUnsubscribe 'Actions' that have subscribed with 'subId'.
```
</details>

<h3 style="font-weight:bold;color: #00BBC6">Two Way Push Pull Notifications</h3>

To enable ***two way*** push notifications, allowing data to be sent out and returned, you can employ the `PushPullReactable<TIn, TOut>` type. Subscribers, on the other hand, utilize the `ReceiveRespondSubscription<TIn, TOut>` when subscribing. This approach proves useful when you need to send a push notification with data required by the receiver, who then responds with data back to the source that initiated the notification.  This is synonymous with sending an email out to a person and getting a response back.

<details closed><summary>Two Way Notification Example</summary>

```cs
var favoriteMessenger = new PushPullReactable<string, string>();
var subId = Guid.NewGuid(); // This is the ID used to identify the event

var unsubscriber = favoriteMessenger.Subscribe(new ReceiveRespondSubscription<string, string>(
    id: subId,
    onRespond: (data) => data switch
        {
            "prog-lang" => "C#",
            "food" => "scotch eggs",
            "past-time" => "game development",
            "music" => "hard rock/metal",
        },
    name: "favorites",
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")
));

Console.WriteLine($"Favorite Language: {favoriteMessenger.PushPull("prog-lang", subId)}");
Console.WriteLine($"Favorite Food: {favoriteMessenger.PushPull("food", subId)}");
Console.WriteLine($"Favorite Past Time: {favoriteMessenger.PushPull("past-time", subId)}");
Console.WriteLine($"Favorite Music: {favoriteMessenger.PushPull("music", subId)}");
```
</details>

<br/>

> [!Note]
> The difference between _**one way**_ and _**two way**_ notifications is that _**one way**_ notifications enable data travel in one direction whereas _**two way**_ notifications enable data travel in both directions.  The terms 'Push', 'Pull', 'Receive', and 'Respond' should give a clue as to the direction of travel of the data.


> [!Tip]
> Most of the time, the built in reactable implementations will suit your needs.  However, if you have any requirements that these can't provide, you can always create your own custom implementations using the interfaces provided.


<h2 style="font-weight:bold;" align="center">🙏🏼 Contributing 🙏🏼</h2>

Interested in contributing? If so, click [here](https://github.com/KinsonDigital/.github/blob/main/docs/CONTRIBUTING.md) to learn how to contribute your time or [here](https://github.com/sponsors/KinsonDigital) if you are interested in contributing your funds via a one-time or recurring donation.

<h2 style="font-weight:bold;border:0" align="center">🔧 Maintainers 🔧</h2>

![x-logo-dark-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-dark-mode.svg#gh-dark-mode-only)
![x-logo-light-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-light-mode.svg#gh-light-mode-only)
[Calvin Wilkinson](https://twitter.com/KDCoder) (KinsonDigital GitHub Organization - Owner)


![x-logo-dark-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-dark-mode.svg#gh-dark-mode-only)
![x-logo-light-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-light-mode.svg#gh-light-mode-only)
[Kristen Wilkinson](https://twitter.com/kswilky) (KinsonDigital GitHub Organization - Project Management, Documentation, Tester)


<h2 style="font-weight:bold;border:0" align="center">🚔 Licensing and Governance 🚔</h2>

<div align="center">


[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg?style=flat)](https://github.com/KinsonDigital/.github/blob/main/docs/code_of_conduct.md)
[![GitHub](https://img.shields.io/github/license/kinsondigital/Carbonate)](https://github.com/KinsonDigital/Carbonate/blob/preview/v1.0.0/LICENSE.md)

<div align= "left">

This software is distributed under the very permissive MIT license and all dependencies are distributed under MIT-compatible licenses.
This project has adopted the code of conduct defined by the **Contributor Covenant** to clarify expected behavior in our community.
