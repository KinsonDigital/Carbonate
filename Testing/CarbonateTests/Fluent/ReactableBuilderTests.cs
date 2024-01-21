// <copyright file="ReactableBuilderTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Fluent;

using Carbonate.Exceptions;
using Carbonate.Fluent;
using Carbonate.NonDirectional;
using Carbonate.OneWay;
using Carbonate.TwoWay;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests the <see cref="ReactableBuilder"/> class.
/// </summary>
public class ReactableBuilderTests
{
    #region Method Tests
    [Fact]
    public void WithId_WithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

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
        var sut = IReactableBuilder.Create();

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
        var sut = IReactableBuilder.Create();

        // Act
        var act = () => sut.WithId(Guid.NewGuid()).WhenUnsubscribing(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onUnsubscribe')");
    }

    [Fact]
    public void WhenUnsubscribing_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var sut = IReactableBuilder.Create()
            .WithId(Guid.NewGuid())
            .WhenUnsubscribing(() => { });

        // Assert
        sut.Should().NotBeNull();
    }

    [Fact]
    public void WithError_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        var act = () => sut.WithId(Guid.NewGuid()).WithError(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onError')");
    }

    [Fact]
    public void BuildPush_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildPush(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }

    [Fact]
    public void BuildOneWayPush_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildOneWayPush<int>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }

    [Fact]
    public void BuildOneWayPull_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildOneWayPull<int>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onRespond')");
    }

    [Fact]
    public void BuildTwoWayPull_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        var act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildTwoWayPull<int, int>(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceiveRespond')");
    }

    [Fact]
    public void BuildPush_WhenInvoked_ShouldReturnCorrectResults()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = IReactableBuilder.Create();

        // Act
        (IDisposable unsubscriber, IPushReactable reactable) = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildPush(() => { });

        // Assert
        unsubscriber.Should().NotBeNull();
        reactable.Should().NotBeNull();
        reactable.Subscriptions.Should().ContainSingle("only one subscription was added.");
        reactable.Subscriptions[0].Id.Should().Be(expectedId);
        reactable.Subscriptions[0].Name.Should().Be("test-name");
        reactable.Subscriptions[0].Unsubscribed.Should().BeFalse();
        reactable.SubscriptionIds.Should().BeEquivalentTo(new[] { expectedId });
    }

    [Fact]
    public void BuildOneWayPush_WhenInvoked_ShouldReturnCorrectResults()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = IReactableBuilder.Create();

        // Act
        (IDisposable unsubscriber, IPushReactable<int> reactable) = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildOneWayPush<int>(_ => { });

        // Assert
        unsubscriber.Should().NotBeNull();
        reactable.Should().NotBeNull();
        reactable.Subscriptions.Should().ContainSingle("only one subscription was added.");
        reactable.Subscriptions[0].Id.Should().Be(expectedId);
        reactable.Subscriptions[0].Name.Should().Be("test-name");
        reactable.Subscriptions[0].Unsubscribed.Should().BeFalse();
        reactable.SubscriptionIds.Should().BeEquivalentTo(new[] { expectedId });
    }

    [Fact]
    public void BuildOneWayPull_WhenInvoked_ShouldReturnCorrectResults()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = IReactableBuilder.Create();

        // Act
        (IDisposable unsubscriber, IPullReactable<int> reactable) = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildOneWayPull(() => 123);

        // Assert
        unsubscriber.Should().NotBeNull();
        reactable.Should().NotBeNull();
        reactable.Subscriptions.Should().ContainSingle("only one subscription was added.");
        reactable.Subscriptions[0].Id.Should().Be(expectedId);
        reactable.Subscriptions[0].Name.Should().Be("test-name");
        reactable.Subscriptions[0].Unsubscribed.Should().BeFalse();
        reactable.SubscriptionIds.Should().BeEquivalentTo(new[] { expectedId });
    }

    [Fact]
    public void BuildTwoWayPull_WhenInvoked_ShouldReturnCorrectResults()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var sut = IReactableBuilder.Create();

        // Act
        (IDisposable unsubscriber, IPushPullReactable<int, int> reactable) = sut
            .WithId(expectedId)
            .WithName("test-name")
            .BuildTwoWayPull<int, int>(_ => 123);

        // Assert
        unsubscriber.Should().NotBeNull();
        reactable.Should().NotBeNull();
        reactable.Subscriptions.Should().ContainSingle("only one subscription was added.");
        reactable.Subscriptions[0].Id.Should().Be(expectedId);
        reactable.Subscriptions[0].Name.Should().Be("test-name");
        reactable.Subscriptions[0].Unsubscribed.Should().BeFalse();
        reactable.SubscriptionIds.Should().BeEquivalentTo(new[] { expectedId });
    }
    #endregion
}
