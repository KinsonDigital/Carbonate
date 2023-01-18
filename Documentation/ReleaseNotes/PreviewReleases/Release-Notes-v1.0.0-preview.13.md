<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.13
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">Breaking Changes 🧨</h2>

1. [#71](https://github.com/KinsonDigital/Carbonate/issues/71) - Removed the following types from the library:
   >💡This was removed due to the types not being required anymore.  Changes to the library have made these types not unnecessary.  
   - `IMessage`
   - `IResult`
   - `MessageFactory`
   - `ResultFactory`
2. [#71](https://github.com/KinsonDigital/Carbonate/issues/71) - Changed all return types for the following interfaces and classes to nullable.
   - `PullReactable<TDataIn, TDataOut>`
   - `IPullable`
   - `RespondReactor`
   - `IPullable`
   - `PullReactable<TDataOut>`

---

<h2 style="font-weight:bold" align="center">Other 🪧</h2>
<h5 align="center">(Includes anything that does not fit into the categories above)</h5>

1. [#67](https://github.com/KinsonDigital/Carbonate/issues/67) - Added XML code docs to NuGet package.
   >💡This will provide documentation in the IDE during development.
2. [#66](https://github.com/KinsonDigital/Carbonate/issues/66) - Updated badges in the project's README file.
