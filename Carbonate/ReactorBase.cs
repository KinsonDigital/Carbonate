// <copyright file="ReactorBase.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using Core;

/// <summary>
/// Provides a mechanism for push or pull based messaging.
/// </summary>
public abstract class ReactorBase : IReactor
{
    private readonly Action? onUnsubscribe;
    private readonly Action<Exception>? onError;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactorBase"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event that was pushed by an <see cref="IReactable{TReactor}"/>.</param>
    /// <param name="name">The name of the <see cref="IReactor"/>.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the provider has finished sending push-based notifications and is unsubscribed.
    /// </param>
    /// <param name="onError">Executed when the provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public ReactorBase(
        Guid eventId,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
    {
        Id = eventId;
        Name = string.IsNullOrEmpty(name) ? string.Empty : name;
        this.onUnsubscribe = onUnsubscribe;
        this.onError = onError;
    }

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public virtual bool Unsubscribed { get; private set; }

    /// <inheritdoc />
    public virtual void OnUnsubscribe()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onUnsubscribe?.Invoke();
        Unsubscribed = true;
    }

    /// <inheritdoc />
    public virtual void OnError(Exception error)
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
}
