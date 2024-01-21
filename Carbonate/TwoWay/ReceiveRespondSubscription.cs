// <copyright file="ReceiveRespondSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.TwoWay;

using System.Diagnostics.CodeAnalysis;
using Core.TwoWay;

/// <inheritdoc cref="IReceiveRespondSubscription{TIn,TOut}"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class ReceiveRespondSubscription<TIn, TOut> : SubscriptionBase, IReceiveRespondSubscription<TIn, TOut>
{
    private readonly Func<TIn, TOut?>? onReceiveRespond;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveRespondSubscription{TIn,TOut}"/> class.
    /// </summary>
    /// <param name="id">The ID of the <see cref="PushPullReactable{TIn,TOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="ReceiveRespondSubscription{TIn,TOut}"/>.</param>
    /// <param name="onReceiveRespond">The delegate to execute to receive data from the source and return data to the source.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the notification provider has finished sending push-based notifications.
    /// </param>
    /// <param name="onError">Executed when the notification provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> parameter is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public ReceiveRespondSubscription(
        Guid id,
        Func<TIn, TOut> onReceiveRespond,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(id, onUnsubscribe, name, onError) => this.onReceiveRespond = onReceiveRespond;

    /// <inheritdoc/>
    public virtual TOut? OnRespond(TIn data)
    {
        if (Unsubscribed)
        {
            return default;
        }

        if (data is null)
        {
            ArgumentNullException.ThrowIfNull(data);
        }

        return this.onReceiveRespond is null ? default : this.onReceiveRespond.Invoke(data);
    }

   /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
