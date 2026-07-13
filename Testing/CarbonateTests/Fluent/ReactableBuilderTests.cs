// <copyright file="ReactableBuilderTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Fluent;

using Carbonate.Exceptions;
using Carbonate.Fluent;
using Carbonate.NonDirectional;
using Carbonate.OneWay;
using Carbonate.TwoWay;
using Shouldly;
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
        Action act = () => sut.WithId(Guid.Empty);

        // Assert
        Should.Throw<EmptySubscriptionIdException>(act).Message.ShouldBe("The subscription ID cannot be empty.");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void WithName_WithNullOrEmptyName_ThrowsException(string? name, string expectedMsg)
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut.WithId(Guid.NewGuid()).WithName(name);

        // Assert
        Should.Throw<ArgumentException>(act).Message.ShouldBe(expectedMsg);
    }

    [Fact]
    public void WhenUnsubscribing_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut.WithId(Guid.NewGuid()).WhenUnsubscribing(null);

        // Assert
        Should.Throw<ArgumentNullException>(act).Message.ShouldBe("Value cannot be null. (Parameter 'onUnsubscribe')");
    }

    [Fact]
    public void WhenUnsubscribing_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var onSubscribingInvoked = false;
        var sut = IReactableBuilder.Create();

        // Act
        (_, IPushReactable reactable) = sut.WithId(Guid.NewGuid())
            .WhenUnsubscribing(() => onSubscribingInvoked = true)
            .BuildPush(() => { });
        reactable.Subscriptions[0].OnUnsubscribe();

        // Assert
        reactable.ShouldNotBeNull();
        onSubscribingInvoked.ShouldBeTrue();
    }

    [Fact]
    public void WithError_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut.WithId(Guid.NewGuid()).WithError(null);

        // Assert
        Should.Throw<ArgumentNullException>(act).Message.ShouldBe("Value cannot be null. (Parameter 'onError')");
    }

    [Fact]
    public void WithError_WhenInvoked_CorrectlySetsOnError()
    {
        // Arrange
        var onErrorInvoked = false;
        var sut = IReactableBuilder.Create();

        // Act
        (_, IPushReactable reactable) = sut
            .WithId(Guid.NewGuid())
            .WithError(_ => onErrorInvoked = true)
            .BuildPush(() => { });
        reactable.Subscriptions[0].OnError(new Exception("test-exception"));

        // Assert
        onErrorInvoked.ShouldBeTrue();
    }

    [Fact]
    public void BuildPush_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut.WithId(Guid.NewGuid()).WithName("test-name").BuildPush(null);

        // Assert
        Should.Throw<ArgumentNullException>(act).Message.ShouldBe("Value cannot be null. (Parameter 'onReceive')");
    }

    [Fact]
    public void BuildOneWayPush_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildOneWayPush<int>(null);

        // Assert
        Should.Throw<ArgumentNullException>(act).Message.ShouldBe("Value cannot be null. (Parameter 'onReceive')");
    }

    [Fact]
    public void BuildOneWayPull_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildOneWayPull<int>(null);

        // Assert
        Should.Throw<ArgumentNullException>(act).Message.ShouldBe("Value cannot be null. (Parameter 'onRespond')");
    }

    [Fact]
    public void BuildTwoWayPull_WithNullParam_ThrowsException()
    {
        // Arrange
        var sut = IReactableBuilder.Create();

        // Act
        Action act = () => sut
            .WithId(Guid.NewGuid())
            .WithName("test-name")
            .BuildTwoWayPull<int, int>(null);

        // Assert
        Should.Throw<ArgumentNullException>(act).Message.ShouldBe("Value cannot be null. (Parameter 'onReceiveRespond')");
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
        unsubscriber.ShouldNotBeNull();
        reactable.ShouldNotBeNull();
        reactable.Subscriptions.ShouldHaveSingleItem("only one subscription was added.");
        reactable.Subscriptions[0].Id.ShouldBe(expectedId);
        reactable.Subscriptions[0].Name.ShouldBe("test-name");
        reactable.Subscriptions[0].Unsubscribed.ShouldBeFalse();
        reactable.SubscriptionIds.ShouldBe(new[] { expectedId });
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
        unsubscriber.ShouldNotBeNull();
        reactable.ShouldNotBeNull();
        reactable.Subscriptions.ShouldHaveSingleItem("only one subscription was added.");
        reactable.Subscriptions[0].Id.ShouldBe(expectedId);
        reactable.Subscriptions[0].Name.ShouldBe("test-name");
        reactable.Subscriptions[0].Unsubscribed.ShouldBeFalse();
        reactable.SubscriptionIds.ShouldBe(new[] { expectedId });
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
        unsubscriber.ShouldNotBeNull();
        reactable.ShouldNotBeNull();
        reactable.Subscriptions.ShouldHaveSingleItem("only one subscription was added.");
        reactable.Subscriptions[0].Id.ShouldBe(expectedId);
        reactable.Subscriptions[0].Name.ShouldBe("test-name");
        reactable.Subscriptions[0].Unsubscribed.ShouldBeFalse();
        reactable.SubscriptionIds.ShouldBe(new[] { expectedId });
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
        unsubscriber.ShouldNotBeNull();
        reactable.ShouldNotBeNull();
        reactable.Subscriptions.ShouldHaveSingleItem("only one subscription was added.");
        reactable.Subscriptions[0].Id.ShouldBe(expectedId);
        reactable.Subscriptions[0].Name.ShouldBe("test-name");
        reactable.Subscriptions[0].Unsubscribed.ShouldBeFalse();
        reactable.SubscriptionIds.ShouldBe(new[] { expectedId });
    }
    #endregion
}
