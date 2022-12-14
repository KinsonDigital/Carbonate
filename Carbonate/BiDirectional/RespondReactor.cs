// <copyright file="RespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.BiDirectional;

using Core;
using Core.BiDirectional;

/// <inheritdoc cref="IRespondReactor{TDataIn,TDataOut}"/>
public sealed class RespondReactor<TDataIn, TDataOut> : ReactorBase, IRespondReactor<TDataIn, TDataOut>
{
    private readonly Func<IMessage<TDataIn>, IResult<TDataOut>?>? onRespondMsg;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondReactor{TDataIn,TDataOut}"/> class.
    /// </summary>
    /// <param name="respondId">The ID of the <see cref="PullReactable{TDataIn,TDataOut}"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondReactor{TDataIn,TDataOut}"/>.</param>
    /// <param name="onRespondMsg">Executed when requesting a response with data.</param>
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
        Func<IMessage<TDataIn>, IResult<TDataOut>?>? onRespondMsg = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(respondId, name, onUnsubscribe, onError) => this.onRespondMsg = onRespondMsg;

    /// <inheritdoc/>
    public IResult<TDataOut> OnRespond(IMessage<TDataIn> message)
    {
        if (Unsubscribed)
        {
            return ResultFactory.CreateEmptyResult<TDataOut>();
        }

        if (message is null)
        {
            throw new ArgumentNullException(nameof(message), "The parameter must not be null.");
        }

        return this.onRespondMsg?.Invoke(message) ?? ResultFactory.CreateEmptyResult<TDataOut>();
    }

   /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
