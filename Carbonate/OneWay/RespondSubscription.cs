// <copyright file="RespondSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using System.Diagnostics.CodeAnalysis;
using Core.OneWay;

/// <inheritdoc cref="IRespondSubscription{TOut}"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class RespondSubscription<TOut> : SubscriptionBase, IRespondSubscription<TOut>
{
    private readonly Func<TOut?>? onRespond;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondSubscription{TOut}"/> class.
    /// </summary>
    /// <param name="id">The ID of the <see cref="IPullReactable{TOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondSubscription{TOut}"/>.</param>
    /// <param name="onRespond">Executed when requesting a response with no data.</param>
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
        Func<TOut> onRespond,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(id, onUnsubscribe, name, onError) => this.onRespond = onRespond;

    /// <inheritdoc />
    public virtual TOut? OnRespond()
    {
        if (Unsubscribed)
        {
            return default;
        }

        return this.onRespond is null ? default : this.onRespond.Invoke();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
