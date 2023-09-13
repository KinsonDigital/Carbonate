// <copyright file="RespondSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.TwoWay;

using System.Diagnostics.CodeAnalysis;
using Core.TwoWay;

/// <inheritdoc cref="IRespondSubscription{TIn,TOut}"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class RespondSubscription<TIn, TOut> : SubscriptionBase, IRespondSubscription<TIn, TOut>
{
    private readonly Func<TIn, TOut?>? onRespond;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondSubscription{TIn,TOut}"/> class.
    /// </summary>
    /// <param name="id">The ID of the <see cref="PushPullReactable{TIn,TOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondSubscription{TIn,TOut}"/>.</param>
    /// <param name="onRespond">Executed when requesting a response with data.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the provider has finished sending push-based notifications and is unsubscribed.
    /// </param>
    /// <param name="onError">Executed when the provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public RespondSubscription(
        Guid id,
        Func<TIn, TOut?>? onRespond = null,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(id, onUnsubscribe, name, onError) => this.onRespond = onRespond;

    /// <inheritdoc/>
    public virtual TOut? OnRespond(TIn data)
    {
        if (Unsubscribed)
        {
            return default;
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data), "The parameter must not be null.");
        }

        return this.onRespond is null ? default : this.onRespond.Invoke(data);
    }

   /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
