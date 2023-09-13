﻿// <copyright file="ReactorUnsubscriberTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Core;

using Carbonate.Core;
using FluentAssertions;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="ReactorUnsubscriber"/> class.
/// </summary>
public class ReactorUnsubscriberTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullReactorsParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ReactorUnsubscriber(null, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'reactors')");
    }

    [Fact]
    public void Ctor_WithNullReactorParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ReactorUnsubscriber(Array.Empty<ISubscription>().ToList(), null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'reactor')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void TotalReactors_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var reactors = new[] { new Mock<ISubscription>().Object, new Mock<ISubscription>().Object };

        var sut = new ReactorUnsubscriber(reactors.ToList(), new Mock<ISubscription>().Object);

        // Act
        var actual = sut.TotalReactors;

        // Assert
        actual.Should().Be(2);
    }

    [Fact]
    public void Dispose_WhenInvoked_RemovesFromReactorsList()
    {
        // Arrange
        var reactorA = new Mock<ISubscription>();
        var reactorB = new Mock<ISubscription>();
        var reactorC = new Mock<ISubscription>();

        var reactors = new[] { reactorA.Object, reactorB.Object, reactorC.Object };

        var sut = new ReactorUnsubscriber(reactors.ToList(), reactorB.Object);

        // Act
        sut.Dispose();
        sut.Dispose();

        var actual = sut.TotalReactors;

        // Assert
        actual.Should().Be(2);
    }
    #endregion
}
