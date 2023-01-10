// <copyright file="IResult.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Contains data that is returned from a <see cref="IReactable{TReactor}"/> subscription.
/// </summary>
/// <typeparam name="T">The type of data that the result holds.</typeparam>
public interface IResult<out T>
{
    /// <summary>
    /// Gets a value indicating whether or not the result is empty.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API")]
    bool IsEmpty { get; }

    /// <summary>
    /// Gets the data as the type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="onError">The action to invoke if an exception occurs.</param>
    /// <typeparam name="T">The type to deserialize the message into.</typeparam>
    /// <returns>The deserialized message data.</returns>
    T? GetValue(Action<Exception>? onError = null);
}
