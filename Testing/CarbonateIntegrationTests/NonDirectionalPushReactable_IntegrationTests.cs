// <copyright file="NonDirectionalPushReactable_IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateIntegrationTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate.NonDirectional;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests all of the components integrated together related to the <see cref="PushReactable"/>.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Integrations Tests Are Named In This Way")]
public class NonDirectionalPushReactable_IntegrationTests
{
    [Fact]
    public void WhenPushing_And_WithSingleEventID_And_WithSingleSubscription_And_WithSingleUnsubscribe_ReturnsCorrectResults()
    {
        // Arrange
        var eventId = new Guid("98a879d4-e819-41da-80e4-a1b459b3e43f");

        IDisposable? unsubscriber = null;

        var reactable = new PushReactable();

        unsubscriber = reactable.Subscribe(new ReceiveSubscription(
            eventId: eventId,
            onReceive: () => { },
            onUnsubscribe: () => unsubscriber?.Dispose()));

        // Act
        reactable.Push(eventId);
        reactable.Unsubscribe(eventId);

        // Assert
        reactable.Reactors.Should().HaveCount(0);
    }
}
