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
        var id = new Guid("98a879d4-e819-41da-80e4-a1b459b3e43f");

        IDisposable? unsubscriber = null;

        var sut = new PushReactable();

        unsubscriber = sut.Subscribe(new ReceiveSubscription(
            id: id,
            onReceive: () => { },
            onUnsubscribe: () => unsubscriber?.Dispose()));

        // Act
        sut.Push(id);
        sut.Unsubscribe(id);

        // Assert
        sut.Subscriptions.Should().HaveCount(0);
    }

    [Fact]
    public void Unsubscribing_BeforeCallingUnsubscribeAll_DoesNotThrowException()
    {
        // Arrange
        var id = new Guid("f227d62c-1830-4a42-a5b3-3c920779bf94");
        IDisposable? unsubscriberB = null;
        var sut = new PushReactable();

        sut.Subscribe(
            new ReceiveSubscription(
                id: id,
                onReceive: () => { },
                onUnsubscribe: () =>
                {
                    // Unsubscribe from the second subscription so when
                    // it comes its turn to be unsubscribed, it will be null.
                    unsubscriberB.Dispose();
                }));

        unsubscriberB = sut.Subscribe(
            new ReceiveSubscription(
                id: id,
                onReceive: () => { },
                onUnsubscribe: () => unsubscriberB.Dispose()));

        // Act
        var act = () => sut.UnsubscribeAll();

        // Assert
        act.Should().NotThrow();
    }
}
