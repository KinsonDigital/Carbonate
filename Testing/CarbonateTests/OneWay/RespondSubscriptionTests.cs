// <copyright file="RespondSubscriptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.OneWay;

using Carbonate.OneWay;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="RespondSubscription{TOut}"/> class.
/// </summary>
public class RespondSubscriptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsIdProperty()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new RespondSubscription<string>(id, () => string.Empty);

        // Assert
        sut.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsName()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new RespondSubscription<string>(id, () => "value", "test-name");

        // Assert
        sut.Name.Should().Be("test-name");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnRespond_WhenUnsubscribed_DoesNotInvokeOnRespondAction()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondSubscription<string>(
            Guid.NewGuid(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return string.Empty;
            },
            "test-name");

        sut.OnUnsubscribe();

        // Act
        _ = sut.OnRespond();

        // Assert
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnRespond_WhenNotUnsubscribed_InvokesOnRespondAction()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondSubscription<string>(
            Guid.NewGuid(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return "return-value";
            },
            "test-name");

        // Act
        _ = sut.OnRespond();

        // Assert
        totalActionInvokes.Should().Be(1);
    }

    [Fact]
    public void OnUnsubscribe_WhenInvoked_UnsubscribesSubscription()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondSubscription<string>(
            Guid.NewGuid(),
            onRespond: () => string.Empty,
            onUnsubscribe: () => totalActionInvokes++);

        // Act
        sut.OnUnsubscribe();
        sut.OnUnsubscribe();

        // Assert
        sut.Unsubscribed.Should().BeTrue();
        totalActionInvokes.Should().Be(1);
    }

    [Fact]
    public void OnError_WhenUnsubscribed_DoesNotInvokedOnErrorAction()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondSubscription<string>(
            Guid.NewGuid(),
            onRespond: () => string.Empty,
            onError: _ => totalActionInvokes++);

        sut.OnUnsubscribe();

        // Act
        sut.OnError(Arg.Any<Exception>());

        // Assert
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnError_WhenNotUnsubscribedAndWithNullParam_ThrowsException()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondSubscription<string>(
            Guid.NewGuid(),
            onRespond: () => string.Empty,
            onError: _ => totalActionInvokes++);

        // Act
        var act = () => sut.OnError(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'error')");
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnError_WhenNotUnsubscribed_InvokesOnErrorAction()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondSubscription<string>(
            Guid.Empty,
            onRespond: () => string.Empty,
            onError: e =>
            {
                e.Should().BeOfType<InvalidOperationException>();
                e.Message.Should().Be("test-exception");

                totalActionInvokes++;
            });

        // Act
        sut.OnError(new InvalidOperationException("test-exception"));

        // Assert
        totalActionInvokes.Should().Be(1);
    }

    [Theory]
    [InlineData("test-value", "4ff67e7b-bdda-4e0c-b34c-0b32270c336d", "test-value - 4ff67e7b-bdda-4e0c-b34c-0b32270c336d")]
    [InlineData(null, "4ff67e7b-bdda-4e0c-b34c-0b32270c336d", "4ff67e7b-bdda-4e0c-b34c-0b32270c336d")]
    [InlineData("", "4ff67e7b-bdda-4e0c-b34c-0b32270c336d", "4ff67e7b-bdda-4e0c-b34c-0b32270c336d")]
    public void ToString_WhenInvoked_ReturnsCorrectResult(
        string name,
        string guid,
        string expected)
    {
        // Arrange
        var id = new Guid(guid);

        var sut = new RespondSubscription<string>(
            id: id,
            onRespond: () => string.Empty,
            name: name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
