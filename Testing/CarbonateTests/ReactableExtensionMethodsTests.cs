// <copyright file="ReactableExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate;
using Carbonate.NonDirectional;
using Carbonate.OneWay;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="ReactableExtensionMethods"/> class.
/// </summary>
public class ReactableExtensionMethodsTests
{
    #region Method Exception Tests
    #region CreateNonReceiveOrRespond With 4 Params
    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable? sut = null;

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), name, Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), "test-name", Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateNonReceiveOrRespond With 5 Params
    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable? sut = null;

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Arg.Any<Guid>(), Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Arg.Any<Guid>(), Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateOneWayReceive With 4 Params
    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, "test-name", _ => { });

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, "test-name", _ => { });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.NewGuid(), name, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.NewGuid(), "test-name", null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateOneWayReceive With 5 Params
    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, _ => { });

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, _ => { });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateOneWayRespond With 4 Params
    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPullReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, "test-name", () => 10);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, "test-name", () => 20);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.NewGuid(), name, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.NewGuid(), "test-name", null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onRespond')");
    }
    #endregion

    #region CreateOneWayRespond With 5 Params
    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPullReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, () => 30);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, () => 40);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoNameAndWithNullOnRespond_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onRespond')");
    }
    #endregion
    #endregion

    #region Method Non-Exception Tests
    [Fact]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndName_CreatesSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedAction = () => { };
        var sut = new PushReactable();

        // Act
        var unsubscriber = sut.CreateNonReceiveOrRespond(id, expectedName, expectedAction);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndAutoName_CreatesSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedAction = () => { };
        var sut = new PushReactable();

        // Act
        var unsubscriber = sut.CreateNonReceiveOrRespond(id, expectedAction);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayReceive_WhenInvokingWithIdAndName_CreatesSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedAction = (int _) => { };
        var sut = new PushReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayReceive(id, expectedName, expectedAction);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoName_CreatesSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedAction = (int _) => { };
        var sut = new PushReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayReceive(id, expectedAction);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayRespond_WhenInvokingWithIdAndName_CreatesSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedAction = () => 123;
        var sut = new PullReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayRespond(id, expectedName, expectedAction);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
    }

    [Fact]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoName_CreatesSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedAction = () => 456;
        var sut = new PullReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayRespond(id, expectedAction);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
    }
    #endregion
}
