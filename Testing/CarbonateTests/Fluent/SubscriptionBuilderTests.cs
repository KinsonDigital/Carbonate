// <copyright file="SubscriptionBuilderTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Fluent;

using Carbonate.Exceptions;
using Carbonate.Fluent;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests the <see cref="SubscriptionBuilder"/> class.
/// </summary>
public class SubscriptionBuilderTests
{
    #region Method Tests
    [Fact]
    public void WithId_WithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut.WithId(Guid.Empty);

        // Assert
        act.Should().Throw<EmptySubscriptionIdException>()
            .WithMessage("The subscription ID cannot be empty.");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void WithName_WithNullOrEmptyName_ThrowsException(string? name, string expectedMsg)
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut.WithId(Guid.NewGuid()).WithName(name);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage(expectedMsg);
    }

    [Fact]
    public void WhenUnsubscribing_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut.WithId(Guid.NewGuid()).WhenUnsubscribing(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onUnsubscribe')");
    }

    [Fact]
    public void WithError_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut.WithId(Guid.NewGuid()).WithError(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onError')");
    }

    [Fact]
    public void BuildNonReceiveOrRespond_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildNonReceiveOrRespond(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }

    [Fact]
    public void BuildOneWayReceive_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildOneWayReceive<int>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }

    [Fact]
    public void BuildOneWayRespond_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildOneWayRespond<int>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onRespond')");
    }

    [Fact]
    public void BuildTwoWay_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = ISubscriptionBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildTwoWay<int, int>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceiveRespond')");
    }

    [Fact]
    public void BuildNonReceiveOrRespond_WhenInvoked_SetsIdAndNameProps()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = ISubscriptionBuilder.Create();

        // Act
        var sub = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildNonReceiveOrRespond(() => { });

        // Assert
        sub.Id.Should().Be(expectedId);
        sub.Name.Should().Be("test-name");
    }

    [Fact]
    public void BuildOneWayReceive_WhenInvoked_SetsIdAndNameProps()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = ISubscriptionBuilder.Create();

        // Act
        var sub = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildOneWayReceive<int>(_ => { });

        // Assert
        sub.Id.Should().Be(expectedId);
        sub.Name.Should().Be("test-name");
    }

    [Fact]
    public void BuildOneWayRespond_WhenInvoked_SetsIdAndNameProps()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = ISubscriptionBuilder.Create();

        // Act
        var sub = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildOneWayRespond(() => 123);

        // Assert
        sub.Id.Should().Be(expectedId);
        sub.Name.Should().Be("test-name");
    }

    [Fact]
    public void BuildTwoWay_WhenInvoked_SetsIdAndNameProps()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = ISubscriptionBuilder.Create();

        // Act
        var sub = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildTwoWay<int, int>(_ => 123);

        // Assert
        sub.Id.Should().Be(expectedId);
        sub.Name.Should().Be("test-name");
    }
    #endregion
}
