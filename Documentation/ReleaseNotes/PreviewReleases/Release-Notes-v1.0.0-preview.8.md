<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.8
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">New Features ✨</h2>

1. [#43](https://github.com/KinsonDigital/Carbonate/issues/43) - Added an exception to the `Reactable` class when invoking the methods below after disposal.
   >💡This gives the benefit of letting the user know what is expected after using the object after invoking `Reactable.Dispose()`. 
   - `Reactable.Subscribe()`
   - `Reactable.Push()`
   - `Reactable.PushData()`
   - `Reactable.PushMessage()`
   - `Reactable.Unsubscribe()`
   - `Reactable.UnsubscribeAll()`
2. [#39](https://github.com/KinsonDigital/Carbonate/issues/39) - Added a new property named `Name` to the `IReactable` interface and `Reactable` class.
   >💡This is used to help with debugging and personal identification.  

   >⚠️This is not used in anyway to identify unique `Reactors` subscribed to the `Reactable`.  That is the purpose of the `EventId` property.
3. [#39](https://github.com/KinsonDigital/Carbonate/issues/39) - Added a default implementation of the `Reactor.ToString()` method that shows the name of the `Reactor` combined with the `EventId`
   >💡If a name is used, the format will be _**'\<name\> - \<event-id\>'**_ and _**'\<event-id\>'**_ if no name is used.

---

<h2 style="font-weight:bold" align="center">Bug Fixes 🐛</h2>

1. [#41](https://github.com/KinsonDigital/Carbonate/issues/41) - Fixed the following bugs:
   - Fixed a bug where the `Reactable.Unsubscribe()` method was not removing the subscription from the internal subscription list.
   - Fixed a bug where a premature unsubscription in `OnNext()` implementation was throwing an exception when invoking the `Reactable.Push()` method.
   - Fixed a bug where a premature unsubscription in `OnNext()` implementation was throwing an exception when invoking the `Reactable.PushData()` method.
   - Fixed a bug where a premature unsubscription in `OnNext()` implementation was throwing an exception when invoking the `Reactable.PushMessage()` method.
   - Fixed a bug where a premature unsubscription in `OnComplete()` implementation was throwing an exception when invoking the `Reactable.Unsubscribe()` method.
   - Fixed a bug where a premature unsubscription in `OnComplete()` implementation was throwing an exception when invoking the `Reactable.UnsubscribeAll()` method.
2. [#40](https://github.com/KinsonDigital/Carbonate/issues/43) - Fixed a bug where invoking the `Reactable.Dispose()` method was throwing an exception.

---

<h2 style="font-weight:bold" align="center">Breaking Changes 🧨</h2>

1. [#41](https://github.com/KinsonDigital/Carbonate/issues/41) - Renamed the `ISerialize` interface to `ISerializeService`.

---

<h2 style="font-weight:bold" align="center">Internal Changes ⚙️</h2>
<h5 align="center">(Changes that do not affect users.  Not breaking changes, new features, or bug fixes.)</h5>

1. [#38](https://github.com/KinsonDigital/Carbonate/issues/38) - Updated the [checkout](https://github.com/marketplace/actions/checkout) GitHub actions in all of the workflows from _**v2**_ to _**v3**_.
