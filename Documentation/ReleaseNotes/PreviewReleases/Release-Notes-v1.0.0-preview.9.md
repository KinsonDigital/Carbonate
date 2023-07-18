<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.9
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">New Features ✨</h2>

1. [#47](https://github.com/KinsonDigital/Carbonate/issues/47) - Added the ability to _**Pull**_ a message from a source.  This is the opposite of the old functionality of doing _**Push**_.  Think of this as requesting data from a source.  The following new types have been added to the library to not only add this functionality but improve upon the original functionality.
   - `IReceiver`
   - `IReceiveReactor`
   - `IResponder`
   - `IRespondReactor`
   - `IResult`
   - `IPullable`
   - `IPushable`
   - `PullReactable`
   - `PushReactable`
   - `ReactableBase`
   - `RespondReactor`
   - `ResultFactory`

---

<h2 style="font-weight:bold" align="center">Breaking Changes 🧨</h2>

1. [#47](https://github.com/KinsonDigital/Carbonate/issues/47) - Made various breaking changes throughout the library.
   - Renamed the following `Reactor` class constructor parameters:
     - Renamed the parameter `onNext` to `onReceive`.
     - Renamed the parameter `onNextMsg` to `onReceiveMsg`.
     - Renamed the parameter `onCompleted` to `onUnsubscribe`.
   - Renamed the interface `IReactor.OnComplete()` method (now the `IReceiveReactor` class), to `OnUnsubscribe()`.
   - Renamed the class `Reactor.OnComplete()` method (now the `ReceiveReactor` class), to `OnUnsubscribe()`.
   - Renamed the class `Reactor` to `ReceiveReactor`.
   - Renamed the class `IReactor` to `IReceiveReactor`.
   - Added a class-level generic to the `IReactable` interface.
     >💡This new generic is constrained to the interface `IReactor`.
   - Moved the following methods in the `IReactable` interface to the new `IPushable` interface:
     - `Push()`
     - `PushData<T>()`
     - `PushMessage()`
   - Renamed the `IReactable.EventIds` property to `SubscriptionIds`.
   - The interface `IReactor` methods `OnNext()` and `OnComplete()` have been removed.
   - The interface `IReactable`
   - Renamed the `Reactable` class to `PushReactable`.
   - Moved the following types to the namespace `Carbonate.Core`:
     - `IReactable`
     - `Reactable`
       >💡Now named `PushReactable`
     - `IReactor`
     - `Reactor`
       >💡Now named `ReceiveReactor`
     - `IMessage`

---

<h2 style="font-weight:bold" align="center">Nuget/Library Updates 📦</h2>

1. [#47](https://github.com/KinsonDigital/Carbonate/issues/47) - Updated, replaced, and removed various NuGet packages.
   - Replaced the NuGet package **NSubstitute** with **Moq** version _**4.18.4**_
     >💡Also updated all of the mocking code in the unit tests project to use **Moq**.
   - Updated the NuGet package **Microsoft.NET.Test.Sdk** from version _**v17.3.2**_ to _**v17.4.1**_ for the integration tests project.
   - Updated the NuGet package **Microsoft.NET.Test.Sdk** from version _**v17.4.0**_ to _**v17.4.1**_ for the unit tests project.

---

<h2 style="font-weight:bold" align="center">Other 🪧</h2>
<h5 align="center">(Includes anything that does not fit into the categories above)</h5>

1. [#47](https://github.com/KinsonDigital/Carbonate/issues/47) - Added a new word to the dictionary for _**JetBrains Rider**_ users.
