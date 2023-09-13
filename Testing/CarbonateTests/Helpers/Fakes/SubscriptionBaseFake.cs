// <copyright file="ReactorBaseFake.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Helpers.Fakes;

using Carbonate;

/// <summary>
/// Used for the purpose of testing the <see cref="SubscriptionBase"/>.
/// </summary>
public class SubscriptionBaseFake : SubscriptionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionBaseFake"/> class.
    /// </summary>
    /// <param name="eventId">Test ID.</param>
    /// <param name="name">Test name.</param>
    /// <param name="onUnsubscribe">Test action for unsubscribing.</param>
    /// <param name="onError">Test action for errors.</param>
    public SubscriptionBaseFake(
        Guid eventId,
        string name = "",
        Action? onUnsubscribe = null,
        Action<Exception>? onError = null)
            : base(eventId, name, onUnsubscribe, onError)
    {
    }
}
