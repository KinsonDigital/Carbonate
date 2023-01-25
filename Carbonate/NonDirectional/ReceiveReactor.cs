// <copyright file="ReceiveReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

using System.Diagnostics.CodeAnalysis;
using Core;
using Core.NonDirectional;

/// <inheritdoc cref="IReceiveReactor"/>
[SuppressMessage(
    "ReSharper",
    "ClassWithVirtualMembersNeverInherited.Global",
    Justification = "Left unsealed to give users more control")]
public class ReceiveReactor : ReactorBase, IReceiveReactor
{
    private readonly Action? onReceive;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveReactor"/> class.
    /// </summary>
    /// <param name="eventId">The ID of the event that was pushed by an <see cref="IReactable{IReceiveReactor}"/>.</param>
    /// <param name="name">The name of the <see cref="ReceiveReactor"/>.</param>
    /// <param name="onReceive">Executed when a push notification occurs with no data.</param>
    /// <param name="onUnsubscribe">
    ///     Executed when the provider has finished sending push-based notifications and is unsubscribed.
    /// </param>
    /// <param name="onError">Executed when the provider experiences an error.</param>
    /// <remarks>
    ///     Note:  The <paramref name="name"/> is not used for unique identification purposes.
    ///     <br/>
    ///     It is only metadata for debugging or miscellaneous purposes.
    /// </remarks>
    public ReceiveReactor(
        Guid eventId,
        string name = "",
        Action? onReceive = null,
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(eventId, name, onUnsubscribe, onError) => this.onReceive = onReceive;

    /// <inheritdoc />
    public virtual void OnReceive()
    {
        if (Unsubscribed)
        {
            return;
        }

        this.onReceive?.Invoke();
    }

   /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Name) ? string.Empty : " - ")}{Id}";
}
