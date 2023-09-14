<h1 align="center" style="color: mediumseagreen;font-weight: bold;">
Carbonate Preview Release Notes - v1.0.0-preview.16
</h1>

<h2 align="center" style="font-weight: bold;">Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

<h2 align="center" style="font-weight: bold;">New Features ✨</h2>

1. [#150](https://github.com/KinsonDigital/Carbonate/issues/150) - Add fluent API to help improve the readability and setup of reactable and subscription setup.
   - The fluent API types are located in a new namespace named `Carbonate.Fluent`.  Use the types `ReactableBuilder` and `SubscriptionBuilder`.

<h2 align="center" style="font-weight: bold;">Breaking Changes 🧨</h2>

1. [#150](https://github.com/KinsonDigital/Carbonate/issues/150) - Introduced many public API improvements related to parameter order, class, interface, and param names.  This was done to help improve the understanding and usage of the API.  Updating your code base is pretty straightforward.
    - Renamed the `TDataIn` generic params in the entire codebase to `TIn`.
    - Renamed the `TDataOut` generic params in the entire codebase to `TOut`.
    - Renamed the `Carbonate.UniDirectional` namespace to `Carbonate.OneWay`.
    - Renamed the `Carbonate.BiDirectional` namespace to `Carbonate.TwoWay`.
    - Renamed the `Carbonate.Core.UniDirectional` namespace to `Carbonate.Core.OneWay`.
    - Renamed the `Carbonate.Core.BiDirectional` namespace to `Carbonate.Core.TwoWay`.
    - Renamed the `Carbonate.BiDirectional.IPullReactable` interface to `Carbonate.TwoWay.IPushPullReactable`.
    - Renamed the `Carbonate.BiDirectional.PullReactable` class to `Carbonate.TwoWay.PushPullReactable`.
    - Renamed the `Carbonate.BiDirectional.IPullable` interface to `Carbonate.TwoWay.IPushablePullable`.
    - Renamed the `ReceiveReactor` constructor param named `onReceiveData` to `onReceive`.
    - Renamed the non-directional `RespondReactor` constructor param named `onRespondData` to `onRespond`.
    - Renamed the one-way `RespondReactor<TDataIn>` constructor param named `onRespondData` to `onReceiveRespond`.
    - Renamed the two-way `RespondReactor<TDataOut, TDataIn>` constructor param named `onRespondData` to `onReceiveRespond`.
    - Renamed the non-directional `IReceiveReactor` interface to `IReceiveSubscription`.
    - Renamed the non-directional `ReceiveReactor` class to `ReceiveSubscription`.
    - Renamed the one-way `IReceiveReactor<TDataIn>` interface to `IReceiveSubscription<TIn>`.
    - Renamed the one-way `ReceiveReactor<TDataIn>` class to `ReceiveSubscription<TIn>`.
    - Renamed the one-way `IRespondReactor<TDataIn>` interface to `IRespondSubscription<TIn>`.
    - Renamed the one-way `RespondReactor<TDataIn>` class to `RespondSubscription<TIn>`.
    - Renamed the one-way `IRespondReactor<TDataIn, TDataOut>` interface to `IRespondSubscription<TIn, TOut>`.
    - Renamed the one-way `RespondReactor<TDataIn, TDataOut>` class to `RespondSubscription<TIn, TOut>`.
    - Renamed the `IReactor` interface to `ISubscription`.
    - Renamed the `IReactable.Reactors` property to `IReactable.Subscriptions`.
    - Renamed the `IReactable.Subscribe()` method parameter named `reactor` to `subscription`.
    - Renamed the `ReactableBase.Reactors` property to `ReactableBase.Subscriptions`
    - Renamed the `ReactableBase.Subscribe()` method parameter named `reactor` to `subscription`.
    - Renamed `ReactableBase` class generic parameter from `T` to `TSubscription`.
    - Renamed the non-directional `ReceiveSubscription` constructor parameter `eventId` to `id`.
    - Renamed the one-way `RespondSubscription<TOut>` constructor parameter `responded` to `id`.
    - Renamed the two-way `RespondSubscription<TIn, TOut>` constructor parameter `responded` to `id`.
    - Renamed the `SubscriptionBase` constructor parameter `eventId` to `id`.
    - Swapped the parameter positions for parameters `name` and `onReceive` for the non-directional `ReceiveSubscription` constructor.
    -    - Used to be the `ReceiveReactor` class
    - Swapped the parameter positions for parameters `name` and `onReceive` for the one-way `ReceiveSubscription<TIn>` constructor.
    -    - Used to be the `ReceiveReactor<TDataIn>` class
    - Swapped the parameter positions for parameters `name` and `onReceive` for the one-way `ReceiveSubscription<TIn>` constructor.
    -    - Used to be the `ReceiveReactor<TDataIn>` class
    - Swapped the parameter positions for parameters `name` and `onRespond` for the one-way `RespondSubscription<TOut>` constructor.
    -    - Used to be the `RespondReactor<TDataOut>` class
    - Swapped the parameter positions for parameters `name` and `onReceiveRespond` for the two-way `RespondSubscription<TIn, TOut>` constructor.
    -    - Used to be the `RespondReactor<TDataIn, TDataOut>` class
    - Set the non-directional `ReceiveSubscription` class constructor parameter named `onReceive` to be non-nullable.
    -    - Used to be the `ReceiveReactor` class
    - Set the one-way `ReceiveSubscription<TIn>` class constructor parameter named `onReceive` to be non-nullable.
    -    - Used to be the `ReceiveReactor<TDataIn>` class
    - Set the one-way `RespondSubscription<TOut>` class constructor parameter named `onRespond` to be non-nullable.
    -    - Used to be the `RespondReactor<TDataOut>` class
    - Set the two-way `RespondSubscription<TIn, TOut>` class constructor parameter named `onReceiveRespond` to be non-nullable.
      - Used to be the `RespondReactor<TDataIn, TDataOut>` class

<h2 align="center" style="font-weight: bold;">Dependency Updates 📦</h2>

1. [#149](https://github.com/KinsonDigital/Carbonate/pull/149) - Updated dependency benchmarkdotnet to _**v0.13.8**_
2. [#147](https://github.com/KinsonDigital/Carbonate/pull/147) - Updated actions/checkout action to _**v4**_
3. [#146](https://github.com/KinsonDigital/Carbonate/pull/146) - Updated kinsondigital/infrastructure action to _**v13**_
4. [#145](https://github.com/KinsonDigital/Carbonate/pull/145) - Updated dependency microsoft.net.test.sdk to _**v17.7.2**_
5. [#143](https://github.com/KinsonDigital/Carbonate/pull/143) - Updated dependency microsoft.codeanalysis.netanalyzers to _**v7.0.4**_
6. [#141](https://github.com/KinsonDigital/Carbonate/pull/141) - Updated dependency fluentassertions to _**v6.12.0**_
7. [#139](https://github.com/KinsonDigital/Carbonate/pull/139) - Updated dependency microsoft.net.test.sdk to _**v17.7.1**_
8. [#138](https://github.com/KinsonDigital/Carbonate/pull/138) - Updated kinsondigital/infrastructure action to _**v12**_
9. [#137](https://github.com/KinsonDigital/Carbonate/pull/137) - Locked version of MOQ.
10. [#135](https://github.com/KinsonDigital/Carbonate/pull/135) - Updated kinsondigital/infrastructure action to _**v11**_
11. [#133](https://github.com/KinsonDigital/Carbonate/pull/133) - Updated dependency microsoft.net.test.sdk to _**v17.7.0**_
12. [#132](https://github.com/KinsonDigital/Carbonate/pull/132) - Updated dependency benchmarkdotnet to _**v0.13.7**_

<h2 align="center" style="font-weight: bold;">Other 🪧</h2>

1. [#151](https://github.com/KinsonDigital/Carbonate/issues/151) - Fixed an issue with the sync status check bot.
2. [#142](https://github.com/KinsonDigital/Carbonate/issues/142) - Adjusted workflow triggers.
3. [#130](https://github.com/KinsonDigital/Carbonate/issues/130) - Adjusted release workflow input.
4. [#126](https://github.com/KinsonDigital/Carbonate/issues/126) - Improved readme.
