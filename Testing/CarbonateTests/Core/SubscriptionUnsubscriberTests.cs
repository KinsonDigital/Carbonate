// <copyright file="SubscriptionUnsubscriberTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Core;

using System.Diagnostics.CodeAnalysis;
using Carbonate.Core;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="SubscriptionUnsubscriber"/> class.
/// </summary>
public class SubscriptionUnsubscriberTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullSubscriptionsParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new SubscriptionUnsubscriber(null, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'subscriptions')");
    }

    [Fact]
    public void Ctor_WithNullSubscriptionParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new SubscriptionUnsubscriber(Array.Empty<ISubscription>().ToList(), null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'subscription')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void TotalSubscriptions_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var subscriptions = new[] { Substitute.For<ISubscription>(), Substitute.For<ISubscription>() };

        var sut = new SubscriptionUnsubscriber(subscriptions.ToList(), Substitute.For<ISubscription>());

        // Act
        var actual = sut.TotalSubscriptions;

        // Assert
        actual.Should().Be(2);
    }

    [Fact]
    [SuppressMessage("csharpsquid", "S3966", Justification = "Required for testing.")]
    public void Dispose_WhenInvoked_RemovesFromSubscriptionsList()
    {
        // Arrange
        var subA = Substitute.For<ISubscription>();
        var subB = Substitute.For<ISubscription>();
        var subC = Substitute.For<ISubscription>();

        var subscriptions = new[] { subA, subB, subC };

        var sut = new SubscriptionUnsubscriber(subscriptions.ToList(), subB);

        // Act
        sut.Dispose();
        sut.Dispose();

        var actual = sut.TotalSubscriptions;

        // Assert
        actual.Should().Be(2);
    }
    #endregion
}
