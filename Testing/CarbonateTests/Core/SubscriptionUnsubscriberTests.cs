// <copyright file="SubscriptionUnsubscriberTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Core;

using System.Diagnostics.CodeAnalysis;
using Carbonate.Core;
using FluentAssertions;
using Moq;
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
        var subscriptions = new[] { new Mock<ISubscription>().Object, new Mock<ISubscription>().Object };

        var sut = new SubscriptionUnsubscriber(subscriptions.ToList(), new Mock<ISubscription>().Object);

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
        var subA = new Mock<ISubscription>();
        var subB = new Mock<ISubscription>();
        var subC = new Mock<ISubscription>();

        var subscriptions = new[] { subA.Object, subB.Object, subC.Object };

        var sut = new SubscriptionUnsubscriber(subscriptions.ToList(), subB.Object);

        // Act
        sut.Dispose();
        sut.Dispose();

        var actual = sut.TotalSubscriptions;

        // Assert
        actual.Should().Be(2);
    }
    #endregion
}
