// <copyright file="IMessage.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// A message that can be passed containing the data to consume.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Deserializes the message to the type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="onError">The action to invoke if an exception occurs.</param>
    /// <typeparam name="T">The type to deserialize the message.</typeparam>
    /// <returns>The deserialized message data.</returns>
    public T? GetData<T>(Action<Exception>? onError = null)
        where T : class;
}
