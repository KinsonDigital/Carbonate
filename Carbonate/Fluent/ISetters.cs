// <copyright file="ISetters.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

/// <summary>
/// Applies various effects to a build result.
/// </summary>
/// <typeparam name="TBuildResult">The build result.</typeparam>
public interface ISetters<out TBuildResult>
{
    /// <summary>
    /// Uses the given <paramref name="name"/> for the final build result.
    /// </summary>
    /// <param name="name">The name to apply.</param>
    /// <returns>
    ///     The build result with the given <paramref name="name"/> applied.
    /// </returns>
    TBuildResult WithName(string name);

    /// <summary>
    /// Uses the given <paramref name="onUnsubscribe"/> <see cref="Action"/> delegate that
    /// would be executed when unsubscribed.
    /// </summary>
    /// <param name="onUnsubscribe">The delegate used for unsubscribing.</param>
    /// <returns>
    ///     The build result with the given <paramref name="onUnsubscribe"/> applied.
    /// </returns>
    TBuildResult WhenUnsubscribing(Action onUnsubscribe);

    /// <summary>
    /// Uses the given <paramref name="onError"/> <see cref="Action{T}"/> delegate that
    /// would be executed when unsubscribed.
    /// </summary>
    /// <param name="onError">The delegate used for error handling.</param>
    /// <returns>
    ///     The build result with the given <paramref name="onError"/> applied.
    /// </returns>
    TBuildResult WithError(Action<Exception> onError);
}
