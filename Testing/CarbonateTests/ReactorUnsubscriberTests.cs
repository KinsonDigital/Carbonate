// <copyright file="ReactorUnsubscriberTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using Carbonate;
using FluentAssertions;
using NSubstitute;
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
            _ = new ReactorUnsubscriber(Array.Empty<IReactor>().ToList(), null);
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
        var reactors = new[] { Substitute.For<IReactor>(), Substitute.For<IReactor>() };

        var sut = new ReactorUnsubscriber(reactors.ToList(), Substitute.For<IReactor>());

        // Act
        var actual = sut.TotalReactors;

        // Assert
        actual.Should().Be(2);
    }

    [Fact]
    public void Dispose_WhenInvoked_RemovesFromReactorsList()
    {
        // Arrange
        var reactorA = Substitute.For<IReactor>();
        var reactorB = Substitute.For<IReactor>();
        var reactorC = Substitute.For<IReactor>();

        var reactors = new[] { reactorA, reactorB, reactorC };

        var sut = new ReactorUnsubscriber(reactors.ToList(), reactorB);

        // Act
        sut.Dispose();
        sut.Dispose();

        var actual = sut.TotalReactors;

        // Assert
        actual.Should().Be(2);
    }
    #endregion
}
