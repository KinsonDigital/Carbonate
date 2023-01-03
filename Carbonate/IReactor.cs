// <copyright file="IReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Provides a mechanism for receiving push-based notifications.
/// </summary>
public interface IReactor
{
    /// <summary>
    /// Gets the ID of the subscription where this <see cref="IReactor"/> should respond.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the name of the <see cref="IReactor"/>.
    /// </summary>
    /// <remarks>
    ///     Note: This is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether or not the <see cref="IReactor"/> has been unsubscribed.
    /// </summary>
    /// <remarks>
    ///     This means that the <see cref="ReceiveReactor"/> will not receive the following notifications:
    ///     <br/>
    ///     <list type="bullet">
    ///         <item><see cref="IReceiveReactor"/>.<see cref="IReceiveReactor.OnReceive()"/></item>
    ///         <item><see cref="IReceiveReactor"/>.<see cref="IReceiveReactor.OnReceive(IMessage)"/></item>
    ///         <item><see cref="OnUnsubscribe"/></item>
    ///         <item><see cref="OnError"/></item>
    ///     </list>
    /// </remarks>
    bool Unsubscribed { get; }

    /// <summary>
    /// Notifies the subscriber that the provider has finished sending push-based notifications and has been unsubscribed.
    /// </summary>
    /// <remarks>
    ///     Will not be invoked more than once.
    /// </remarks>
    void OnUnsubscribe();

    /// <summary>
    /// Notifies the subscriber that the provider has experienced an error condition.
    /// </summary>
    /// <param name="error">An object that provides additional information about the error.</param>
    void OnError(Exception error);
}
