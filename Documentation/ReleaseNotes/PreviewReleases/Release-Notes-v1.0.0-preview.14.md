<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.14
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">New Features ✨</h2>

1. [#77](https://github.com/KinsonDigital/Carbonate/issues/77) - Changed the following classes:
   - Removed the `sealed` keyword from the following classes:
        >💡This was done to give users more control.
     - `NonDirectional.ReceiveReactor`
     - `UniDirectional.ReceiveReactor`
     - `UniDirectional.RespondReactor`
     - `BiDirectional.RespondReactor`
   - Change the following methods to `virtual`:
        >💡This was done to give users more control.
     - `NonDirectional.ReceiveReactor.OnReceive()`
     - `UniDirectional.ReceiveReactor.OnReceive()`
     - `UniDirectional.RespondReactor.OnRespond()`
     - `BiDirectional.RespondReactor.OnRespond()`
