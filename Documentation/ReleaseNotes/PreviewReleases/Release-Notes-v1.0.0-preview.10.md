<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.10
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">New Features ✨</h2>

1. [#51](https://github.com/KinsonDigital/Carbonate/issues/51) - Added the 2 interfaces below:
   >💡The classes `PushReactable` and `PullReactable` now inherit these interfaces respectively which  
   > give the ability to use the interface types instead of the class types and still get access to the push and pull methods.  This also increases testability and mocking.
   - `IPushReactable`
   - `IPullReactable`
