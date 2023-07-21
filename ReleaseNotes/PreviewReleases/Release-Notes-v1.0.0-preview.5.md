<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.5
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">Breaking Changes 🧨</h2>

1. [#24](https://github.com/KinsonDigital/Carbonate/issues/24) - Changed the name of the `Reactable.Push()` method overloads.  This was done to improve overloaded method resolution.
   >💡These methods are not overloaded anymore.  One of them has bee named to `PushData<T>()` and the other to `PushMessage()`.
