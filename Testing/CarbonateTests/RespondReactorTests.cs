// <copyright file="RespondReactorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using Carbonate;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="RespondReactor"/> class.
/// </summary>
public class RespondReactorTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsIdProperty()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new RespondReactor(id);

        // Assert
        sut.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsName()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new RespondReactor(id, "test-name");

        // Assert
        sut.Name.Should().Be("test-name");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnRespond_WithNoMessageAndWhenUnsubscribed_ReturnsDefaultResultAndDoesNotInvokeOnRespondAction()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return Arg.Any<IResult>();
            });

        sut.OnUnsubscribe();

        // Act
        var actual = sut.OnRespond();

        // Assert
        actual.Should().BeNull();
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnRespond_WithNoMessageAndWhenNotUnsubscribed_ReturnsCorrectResultAndInvokesOnRespondAction()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return Substitute.For<IResult>();
            });

        // Act
        var actual = sut.OnRespond();

        // Assert
        actual.Should().NotBeNull();
        totalActionInvokes.Should().Be(1);
    }

    [Fact]
    public void OnRespond_WithNonNullMessageAndWhenUnsubscribed_ReturnsDefaultResultAndDoesNotInvokeOnRespondAction()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return Arg.Any<IResult>();
            });

        sut.OnUnsubscribe();

        // Act
        var actual = sut.OnRespond(Substitute.For<IMessage>());

        // Assert
        actual.Should().BeNull();
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnRespond_WithNullMessage_ThrowsException()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            onRespondMsg: _ =>
            {
                totalActionInvokes++;
                return Substitute.For<IResult>();
            });

        // Act
        var act = () => sut.OnRespond(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnRespond_WithNonNullMessageAndNotUnsubscribed_ReturnsCorrectResult()
    {
        // Arrange
        var totalActionInvokes = 0;
        var message = Substitute.For<IMessage>();
        var result = Substitute.For<IResult>();

        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            onRespondMsg: msg =>
            {
                msg.Should().BeSameAs(message);
                totalActionInvokes++;
                return result;
            });

        // Act
        var actual = sut.OnRespond(message);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeSameAs(result);
        totalActionInvokes.Should().Be(1);
    }

    [Fact]
    public void OnUnsubscribe_WhenInvoked_UnsubscribesReactor()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
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

        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
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

        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
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

        var sut = new RespondReactor(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
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
    #endregion
}
