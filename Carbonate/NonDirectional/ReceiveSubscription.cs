﻿// <copyright file="ReceiveSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

using System.Diagnostics.CodeAnalysis;
using Core;
using Core.NonDirectional;

/// <inheritdoc cref="IReceiveSubscription"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class ReceiveSubscription : SubscriptionBase, IReceiveSubscription
{
    private readonly Action? onReceive;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveSubscription"/> class.
    /// </summary>
    /// <param name="id">The ID of the event that was pushed by an <see cref="IReactable{TSubscription}"/>.</param>
    /// <param name="name">The name of the <see cref="ReceiveSubscription"/>.</param>
    /// <param name="onReceive">Executed when a push notification occurs with no data.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> parameter is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public ReceiveSubscription(
        Guid id,
        Action onReceive,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(id, onUnsubscribe, name, onError) => this.onReceive = onReceive;

    /// <inheritdoc />
    public virtual void OnReceive()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onReceive?.Invoke();
    }

   /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
