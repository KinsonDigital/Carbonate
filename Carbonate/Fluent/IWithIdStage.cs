// <copyright file="IWithIdStage.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

/// <summary>
/// Represents a stage in the fluent builder pattern that allows setting the id of the build result.
/// </summary>
/// <typeparam name="TBuildResult">The build result.</typeparam>
public interface IWithIdStage<out TBuildResult>
{
    /// <summary>
    /// Uses the given <paramref name="newId"/> for the final build result.
    /// </summary>
    /// <param name="newId">The id to apply.</param>
    /// <returns>
    ///     The build result with the given <paramref name="newId"/> applied.
    /// </returns>
    public TBuildResult WithId(Guid newId);
}
