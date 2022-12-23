<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.6
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">New Features ✨</h2>

1. [#29](https://github.com/KinsonDigital/Carbonate/issues/29) - Added a new method with the name `Push()` to the `IReactable` interface and `Reactable` class.
   >💡This new method is for pushing a notification without any data.
2. [#29](https://github.com/KinsonDigital/Carbonate/issues/29) - Added a new parameter to the `Reactor` class constructor.
   >💡This is the implementation to invoke when the new `IReactable.Push()` method is invoked.

---

<h2 style="font-weight:bold" align="center">Internal Changes ⚙️</h2>
<h5 align="center">(Changes that do not affect users.  Not breaking changes, new features, or bug fixes.)</h5>

1. [#29](https://github.com/KinsonDigital/Carbonate/issues/29) - Added integration and unit tests
