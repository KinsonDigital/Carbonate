// <copyright file="MessageFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Diagnostics.CodeAnalysis;
using Core;
using Services;

/// <summary>
/// Creates <see cref="IMessage{TDataIn}"/> objects for sending notifications.
/// </summary>
[ExcludeFromCodeCoverage]
public static class MessageFactory
{
    private static readonly ISerializerService SerializerService = new JsonSerializerService();

    /// <summary>
    /// Creates an <see cref="IMessage{TDataIn}"/> object that wraps the given <paramref name="data"/> for
    /// responding to a pull data request or a push notification.
    /// </summary>
    /// <param name="data">The data to send out with the message.</param>
    /// <typeparam name="T">The type of data to package.</typeparam>
    /// <returns>The result that contains the data.</returns>
    /// <returns>
    ///     The data can be retrieved from the result by using <see cref="IMessage{TDataIn}"/>.<see cref="IMessage{TDataIn}.GetData"/>.
    /// </returns>
    public static IMessage<T> CreateMessage<T>(T data)
    {
        var jsonData = SerializerService.Serialize(data);
        return new JsonMessage<T>(SerializerService, jsonData);
    }
}
