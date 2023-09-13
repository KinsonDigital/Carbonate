// <copyright file="ISubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

/// <summary>
/// Provides a mechanism for pushing notifications or receiving responses.
/// </summary>
public interface ISubscription
{
    /// <summary>
    /// Gets the ID of the subscription where this <see cref="ISubscription"/> should respond.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the name of the <see cref="ISubscription"/>.
    /// </summary>
    /// <remarks>
    ///     Note: This is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether or not the <see cref="ISubscription"/> has been unsubscribed.
    /// </summary>
    /// <remarks>
    ///     This means that the <see cref="ISubscription"/> will not receive.
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
