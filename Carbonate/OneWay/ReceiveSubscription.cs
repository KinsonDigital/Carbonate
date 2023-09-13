// <copyright file="ReceiveSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using System.Diagnostics.CodeAnalysis;
using Core;
using Core.OneWay;

/// <inheritdoc cref="IReceiveSubscription{TIn}"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class ReceiveSubscription<TIn> : SubscriptionBase, IReceiveSubscription<TIn>
{
    private readonly Action<TIn>? onReceive;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveSubscription{TIn}"/> class.
    /// </summary>
    /// <param name="id">The ID of the event that was pushed by an <see cref="IReactable{TSubscription}"/>.</param>
    /// <param name="name">The name of the <see cref="ReceiveSubscription{TIn}"/>.</param>
    /// <param name="onReceive">Executed when a push notification occurs with some data.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the provider has finished sending push-based notifications and is unsubscribed.
    /// </param>
    /// <param name="onError">Executed when the provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public ReceiveSubscription(
        Guid id,
        string name = "",
        Action<TIn>? onReceive = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(id, name, onUnsubscribe, onError) => this.onReceive = onReceive;

    /// <inheritdoc />
    public virtual void OnReceive(TIn data)
    {
        if (Unsubscribed)
        {
            return;
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data), "The parameter must not be null.");
        }

        this.onReceive?.Invoke(data);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
