// <copyright file="ReactorBaseFake.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Helpers.Fakes;

using Carbonate;

/// <summary>
/// Used for the purpose of testing the <see cref="ReactorBase"/>.
/// </summary>
public class ReactorBaseFake : ReactorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactorBaseFake"/> class.
    /// </summary>
    /// <param name="eventId">Test ID.</param>
    /// <param name="name">Test name.</param>
    /// <param name="onUnsubscribe">Test action for unsubscribing.</param>
    /// <param name="onError">Test action for errors.</param>
    public ReactorBaseFake(
        Guid eventId,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(eventId, name, onUnsubscribe, onError)
    {
    }
}
