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
    /// <param name="onReceive">The delegate to execute to receive data from the source.</param>
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
        Action<TIn> onReceive,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(id, onUnsubscribe, name, onError) => this.onReceive = onReceive;

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
