// <copyright file="IMessage.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

/// <summary>
/// A message that can be passed containing the data to consume.
/// </summary>
/// <typeparam name="T">The type data returned from the message.</typeparam>
public interface IMessage<out T>
{
    /// <summary>
    /// Gets the data as the type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="onError">The action to invoke if an exception occurs.</param>
    /// <returns>The deserialized message data.</returns>
    public T? GetData(Action<Exception>? onError = null);
}
