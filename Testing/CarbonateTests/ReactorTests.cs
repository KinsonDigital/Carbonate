// <copyright file="ReactorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using Carbonate;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="Reactor"/> class.
/// </summary>
public class ReactorTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsEventId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var sut = new Reactor(guid);
        var actual = sut.EventId;

        // Assert
        actual.Should().Be(guid);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnReceive_WhenSendingNothingAndSubscribed_InvokesAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive() => onReceiveInvoked = true;

        var sut = new Reactor(Guid.NewGuid(), onReceive: OnReceive);

        // Act
        sut.OnReceive();

        // Assert
        onReceiveInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnReceive_WhenSendingNothingAndNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive() => onReceiveInvoked = true;

        var sut = new Reactor(Guid.NewGuid(), onReceive: OnReceive);

        sut.OnUnsubscribe();

        // Act
        sut.OnReceive();

        // Assert
        onReceiveInvoked.Should().BeFalse();
    }

    [Fact]
    public void OnReceive_WhenSendingMessageAndSubscribed_InvokesAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive(IMessage msg) => onReceiveInvoked = true;

        var mockMessage = Substitute.For<IMessage>();

        var sut = new Reactor(Guid.NewGuid(), onReceiveMsg: OnReceive);

        // Act
        sut.OnReceive(mockMessage);

        // Assert
        onReceiveInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnReceive_WhenSendingMessageAndNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive(IMessage msg) => onReceiveInvoked = true;

        var mockMessage = Substitute.For<IMessage>();

        var sut = new Reactor(Guid.NewGuid(), onReceiveMsg: OnReceive);

        sut.OnUnsubscribe();

        // Act
        sut.OnReceive(mockMessage);

        // Assert
        onReceiveInvoked.Should().BeFalse();
    }

    [Fact]
    public void OnUnsubscribe_WhenNotUnsubscribed_InvokesAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnUnsubscribe() => onReceiveInvoked = true;

        var sut = new Reactor(Guid.NewGuid(), onUnsubscribe: OnUnsubscribe);

        // Act
        sut.OnUnsubscribe();

        // Assert
        onReceiveInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnUnsubscribe_WhenUnsubscribed_DoesNotInvokeActionAgain()
    {
        // Arrange
        var totalInvokes = 0;
        void OnUnsubscribe() => totalInvokes++;

        var sut = new Reactor(Guid.NewGuid(), onUnsubscribe: OnUnsubscribe);
        sut.OnUnsubscribe();

        // Act
        sut.OnUnsubscribe();

        // Assert
        totalInvokes.Should().Be(1);
    }

    [Fact]
    public void OnError_WhenNotUnsubscribed_InvokesAction()
    {
        // Arrange
        var onErrorInvoked = false;
        void OnError(Exception ex) => onErrorInvoked = true;

        var exception = new Exception("test-exception");

        var sut = new Reactor(Guid.NewGuid(), onError: OnError);

        // Act
        sut.OnError(exception);

        // Assert
        onErrorInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnError_WhenNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnError(Exception ex) => onReceiveInvoked = true;

        var exception = new Exception("test-exception");

        var sut = new Reactor(Guid.NewGuid(), onError: OnError);

        sut.OnUnsubscribe();

        // Act
        sut.OnError(exception);

        // Assert
        onReceiveInvoked.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    [InlineData("", "5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    [InlineData("test-value", "test-value - 5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    public void ToString_WhenInvoked_ReturnsCorrectResult(string name, string expected)
    {
        // Arrange
        var id = new Guid("5739afd9-be4c-4402-a12d-6bcde35cc8c3");

        var sut = new Reactor(
            eventId: id,
            name: name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
