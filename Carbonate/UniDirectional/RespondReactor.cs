// <copyright file="RespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.UniDirectional;

using Core.UniDirectional;

/// <inheritdoc cref="IRespondReactor{TDataOut}"/>
public sealed class RespondReactor<TDataOut> : ReactorBase, IRespondReactor<TDataOut>
{
    private readonly Func<TDataOut?>? onRespond;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondReactor{TDataOut}"/> class.
    /// </summary>
    /// <param name="respondId">The ID of the <see cref="IPullReactable{TDataOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondReactor{TDataOut}"/>.</param>
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
    public RespondReactor(
        Guid respondId,
        string name = "",
        Func<TDataOut?>? onRespond = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(respondId, name, onUnsubscribe, onError) => this.onRespond = onRespond;

    /// <inheritdoc />
    public TDataOut? OnRespond()
    {
        if (Unsubscribed)
        {
            return default(TDataOut);
        }

        return this.onRespond is null ? default(TDataOut) : this.onRespond.Invoke();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
