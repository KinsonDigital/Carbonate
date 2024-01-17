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
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullAction_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), "test-name", Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'action')");
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
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithNullAction_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), Arg.Any<Action>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'action')");
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
    #endregion
}
