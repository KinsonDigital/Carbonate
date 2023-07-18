<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    Carbonate Preview Release Notes - v1.0.0-preview.12
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

<div align="center">

## ⚠️WARNING!!⚠️
</div>

<div align="center">

This release contains _**MANY**_ breaking changes and the list below is not exhaustive. The purpose of these changes was to enable better usability, flexibility, testability, and type safety.
</div>

---

<h2 style="font-weight:bold" align="center">Breaking Changes 🧨</h2>

1. [#59](https://github.com/KinsonDigital/Carbonate/issues/59) - The following breaking changes were introduced:
   >💡This list is not exhaustive
   - Created the following 6 namespaces for better organization and to help increase understanding of the data flow of the various classes and interfaces.  The majority of the classes and interfaces in the library have been moved to these namespaces.  Each class and interfaces was moved into the correct namespace that matches the direction that data may or may not flow.
     - `Carbonate.Core.NonDirectional`
     - `Carbonate.Core.UniDirectional`
     - `Carbonate.Core.BiDirectional`
     - `Carbonate.NonDirectional`
     - `Carbonate.UniDirectional`
     - `Carbonate.BiDirectional`
   - Moved the majority of the interfaces and classes in the library to one of the new interfaces mentioned above.
   - Added generic parameters to various interfaces and the associated class implementations.
