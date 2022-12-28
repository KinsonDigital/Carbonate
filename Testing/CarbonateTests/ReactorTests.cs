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
    public void OnNext_WhenSendingNothingAndSubscribed_InvokesAction()
    {
        // Arrange
        var onNextInvoked = false;
        void OnNext() => onNextInvoked = true;

        var sut = new Reactor(Guid.NewGuid(), onNext: OnNext);

        // Act
        sut.OnNext();

        // Assert
        onNextInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnNext_WhenSendingNothingAndNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onNextInvoked = false;
        void OnNext() => onNextInvoked = true;

        var sut = new Reactor(Guid.NewGuid(), onNext: OnNext);

        sut.OnComplete();

        // Act
        sut.OnNext();

        // Assert
        onNextInvoked.Should().BeFalse();
    }

    [Fact]
    public void OnNext_WhenSendingMessageAndSubscribed_InvokesAction()
    {
        // Arrange
        var onNextInvoked = false;
        void OnNext(IMessage msg) => onNextInvoked = true;

        var mockMessage = Substitute.For<IMessage>();

        var sut = new Reactor(Guid.NewGuid(), onNextMsg: OnNext);

        // Act
        sut.OnNext(mockMessage);

        // Assert
        onNextInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnNext_WhenSendingMessageAndNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onNextInvoked = false;
        void OnNext(IMessage msg) => onNextInvoked = true;

        var mockMessage = Substitute.For<IMessage>();

        var sut = new Reactor(Guid.NewGuid(), onNextMsg: OnNext);

        sut.OnComplete();

        // Act
        sut.OnNext(mockMessage);

        // Assert
        onNextInvoked.Should().BeFalse();
    }

    [Fact]
    public void OnComplete_WhenNotUnsubscribed_InvokesAction()
    {
        // Arrange
        var onNextInvoked = false;
        void OnComplete() => onNextInvoked = true;

        var sut = new Reactor(Guid.NewGuid(), onCompleted: OnComplete);

        // Act
        sut.OnComplete();

        // Assert
        onNextInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnComplete_WhenUnsubscribed_DoesNotInvokeActionAgain()
    {
        // Arrange
        var totalInvokes = 0;
        void OnComplete() => totalInvokes++;

        var sut = new Reactor(Guid.NewGuid(), onCompleted: OnComplete);
        sut.OnComplete();

        // Act
        sut.OnComplete();

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
        var onNextInvoked = false;
        void OnError(Exception ex) => onNextInvoked = true;

        var exception = new Exception("test-exception");

        var sut = new Reactor(Guid.NewGuid(), onError: OnError);

        sut.OnComplete();

        // Act
        sut.OnError(exception);

        // Assert
        onNextInvoked.Should().BeFalse();
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
