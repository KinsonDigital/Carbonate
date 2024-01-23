<h1 align="center" style="color: mediumseagreen;font-weight: bold;">
Carbonate Preview Release Notes - v1.0.0-preview.17
</h1>

<h2 align="center" style="font-weight: bold;">Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

<h2 align="center" style="font-weight: bold;">New Features ✨</h2>

1. [#188](https://github.com/KinsonDigital/Carbonate/issues/188) - Added a property which is a list of all the subscription names for all of the reactables.
    - The name of the property that was added is `SubscriptionNames`.
2. [#181](https://github.com/KinsonDigital/Carbonate/issues/181) - Added ability to prevent unsubscription in all of the following notification delegates:
   - `onOnReceive`
   - `onOnRespond`
   - `onOnReceiveRespond`
3. [#159](https://github.com/KinsonDigital/Carbonate/issues/159) - Created extension methods for all reactable types to easily create subscriptions.

<h2 align="center" style="font-weight: bold;">Bug Fixes 🐛</h2>

1. [#187](https://github.com/KinsonDigital/Carbonate/issues/187) - Fixed the following bugs:
    - Fixed a processing error in the `TwoWay.PushPullReactable` class.
    - Fixed a processing error in the `OneWay.PullReactable` class.
    - Fixed an issue where it was allowed to pull data while the `OneWay.PullReactable` was disposed.
    - Fixed an issue where it was allowed to push and pull data while the `TwoWay.PushPullReactable` was disposed.

<h2 align="center" style="font-weight: bold;">Breaking Changes 🧨</h2>

1. [#188](https://github.com/KinsonDigital/Carbonate/issues/188) - Introduced the following breaking changes:
    - Changed the `IReactable<TSubscription>.SubscriptionIds` property type from `ReadOnlyCollection<Guid>` to `ImmutableArray<Guid>`.
    - Changed the `ReactableBase<TSubscription>.SubscriptionIds` property type from `ReadOnlyCollection<Guid>` to `ImmutableArray<Guid>`. 
2. [#187](https://github.com/KinsonDigital/Carbonate/issues/187) - Introduced the following breaking changes:
    - Renamed the method parameter of type `Guid` for the following methods to the name `id`:
        -  `NonDirectional.IPushable.Push()`
        -  `NonDirectional.PushReactable.Push()`
        -  `OneWay.IPushable.Push()`
        -  `OneWay.IPullable.Pull()`
        -  `OneWay.PushReactable.Push()`
        -  `OneWay.PullReactable.Pull()`
        -  `TwoWay.IPushablePullable.PushPull()`
        -  `TwoWay.PushPullReactable.PushPull()`
    - Changed the type of the `ReactableBase.Subscriptions` property from `ReadOnlyCollection<TSubscription>` to `ImmutableArray<TSubscription>`.
    - Swapped the position of the parameters for the `Push()` method for the interface `IPushable<TIn>`.
    - Swapped the position of the parameters for the `Push()` method for the class `PushReactable<TIn>`.
    - Swapped the position of the parameters for the `PushPull()` method for the interface `PushablePullable<TIn, TOut>`.
    - Swapped the position of the parameters for the `PushPull()` method for the class `PushPullReactable<TIn, TOut>`.
3. [#159](https://github.com/KinsonDigital/Carbonate/issues/159) - Introduced the following breaking changes:
    - Refactored the name of the `BuildNonReceive()` method to `BuildNonReceiveOrRespond()` in the interface `ISubscriptionBuilder`.
    - Refactored the name of the `BuildNonReceive()` method to `BuildNonReceiveOrRespond()` in the class `SubscriptionBuilder`.
    - Refactored the name of the `BuildTwoWayRespond()` method to `BuildTwoWay()` in the interface `ISubscriptionBuilder`.
    - Refactored the name of the `BuildTwoWayRespond()` method to `BuildTwoWay()` in the class `SubscriptionBuilder`.
    - Refactored the name of the `IRespondSubscription` interface to `IReceiveRespondSubscription`.
    - Refactored the name of the `RespondSubscription` class to `ReceiveRespondSubscription`.

<h2 align="center" style="font-weight: bold;">Dependency Updates 📦</h2>

1. [#185](https://github.com/KinsonDigital/Carbonate/pull/185) - Updated _**dotnet**_ to _**v8**_
2. [#195](https://github.com/KinsonDigital/Carbonate/pull/195) - Updated _**xunit**_ to _**v2.6.6**_
3. [#178](https://github.com/KinsonDigital/Carbonate/pull/178) - Updated _**xunit-runner.visualstudio**_ to _**v2.5.6**_
4.  [#193](https://github.com/KinsonDigital/Carbonate/pull/193) - Updated _**actions/checkout**_ action to _**v4**_
5.  [#171](https://github.com/KinsonDigital/Carbonate/pull/171) - Updated _**actions/setup-dotnet**_ action to _**v4**_
6.  [#177](https://github.com/KinsonDigital/Carbonate/pull/177) - Updated _**kinsondigital/infrastructure**_ action to _**v13.6.3**_
7.  [#180](https://github.com/KinsonDigital/Carbonate/pull/180) - Updated _**benchmarkdotnet**_ to _**v0.12.13**_
8.  [#167](https://github.com/KinsonDigital/Carbonate/pull/167) - Updated _**microsoft.codeanalysis.netanalyzers**_ to _**v8**_
9.  [#166](https://github.com/KinsonDigital/Carbonate/pull/166) - Updated _**microsoft.net.test.sdk**_ to _**v17.8.0**_

<h2 align="center" style="font-weight: bold;">Other 🪧</h2>

1. [#184](https://github.com/KinsonDigital/Carbonate/issues/184) - Improved and updated the sonar scan status check.
2. [#161](https://github.com/KinsonDigital/Carbonate/issues/161) - Updated the sync workflow.
3. [#127](https://github.com/KinsonDigital/Carbonate/issues/127) - Improved the social preview & images.
4. [#187](https://github.com/KinsonDigital/Carbonate/issues/187) - Added a small performance improvement when unsubscribing all of the reactables subscriptions when using the `IReactable.Unsubscribe()` implementation.
