// <copyright file="RespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using Core;

/// <inheritdoc/>
public class RespondReactor : IRespondReactor
{
    private readonly Func<IResult?>? onRespond;
    private readonly Func<IMessage, IResult?>? onRespondMsg;
    private readonly Action? onUnsubscribe;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="RespondReactor"/> class.
    /// </summary>
    /// <param name="respondId">The ID of the <see cref="PullReactable"/> requiring a response.</param>
    /// <param name="name">The name of the <see cref="RespondReactor"/>.</param>
    /// <param name="onRespond">Executed when requesting a response with no data.</param>
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
        Func<IResult?>? onRespond = null,
        Func<IMessage, IResult?>? onRespondMsg = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        Id = respondId;
        Name = string.IsNullOrEmpty(name) ? string.Empty : name;
        this.onRespond = onRespond;
        this.onRespondMsg = onRespondMsg;
        this.onUnsubscribe = onUnsubscribe;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Unsubscribed { get; private set; }

    /// <inheritdoc />
    public IResult? OnRespond() => Unsubscribed ? default : this.onRespond?.Invoke();

    /// <inheritdoc />
    public IResult? OnRespond(IMessage message)
    {
        if (Unsubscribed)
        {
            return default;
        }

        if (message is null)
        {
            throw new ArgumentNullException(nameof(message), "The parameter must not be null.");
        }

        return this.onRespondMsg?.Invoke(message);
    }

    /// <inheritdoc />
    public void OnUnsubscribe()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onUnsubscribe?.Invoke();
        Unsubscribed = true;
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        if (Unsubscribed)
        {
            return;
        }

        if (error is null)
        {
            throw new ArgumentNullException(nameof(error), "The parameter must not be null.");
        }

        this.onError?.Invoke(error);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
