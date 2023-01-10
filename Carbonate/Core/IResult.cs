// <copyright file="IResult.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

/// <summary>
/// Contains data that is returned from a <see cref="IRespondReactor"/> and <see cref="PullReactable"/>.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets the data as the type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="onError">The action to invoke if an exception occurs.</param>
    /// <typeparam name="T">The type to deserialize the message into.</typeparam>
    /// <returns>The deserialized message data.</returns>
    public T? GetValue<T>(Action<Exception>? onError = null)
        where T : class;
}
