﻿// <copyright file="RespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.TwoWay;

using System.Diagnostics.CodeAnalysis;
using Core.TwoWay;

/// <inheritdoc cref="IRespondReactor{TIn,TOut}"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class RespondReactor<TIn, TOut> : ReactorBase, IRespondReactor<TIn, TOut>
{
    private readonly Func<TIn, TOut?>? onRespond;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondReactor{TIn,TOut}"/> class.
    /// </summary>
    /// <param name="respondId">The ID of the <see cref="PushPullReactable{TIn,TOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondReactor{TIn,TOut}"/>.</param>
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
    public RespondReactor(
        Guid respondId,
        string name = "",
        Func<TIn, TOut?>? onRespond = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(respondId, name, onUnsubscribe, onError) => this.onRespond = onRespond;

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
