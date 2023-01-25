// <copyright file="RespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.BiDirectional;

using System.Diagnostics.CodeAnalysis;
using Core.BiDirectional;

/// <inheritdoc cref="IRespondReactor{TDataIn,TDataOut}"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class RespondReactor<TDataIn, TDataOut> : ReactorBase, IRespondReactor<TDataIn, TDataOut>
{
    private readonly Func<TDataIn, TDataOut?>? onRespondData;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondReactor{TDataIn,TDataOut}"/> class.
    /// </summary>
    /// <param name="respondId">The ID of the <see cref="PullReactable{TDataIn,TDataOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondReactor{TDataIn,TDataOut}"/>.</param>
    /// <param name="onRespondData">Executed when requesting a response with data.</param>
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
        Func<TDataIn, TDataOut?>? onRespondData = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(respondId, name, onUnsubscribe, onError) => this.onRespondData = onRespondData;

    /// <inheritdoc/>
    public virtual TDataOut? OnRespond(TDataIn data)
    {
        if (Unsubscribed)
        {
            return default(TDataOut);
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data), "The parameter must not be null.");
        }

        return this.onRespondData is null ? default(TDataOut) : this.onRespondData.Invoke(data);
    }

   /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
