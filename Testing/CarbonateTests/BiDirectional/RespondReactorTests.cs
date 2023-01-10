// <copyright file="RespondReactorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.BiDirectional;

using Carbonate.BiDirectional;
using Carbonate.Core;
using FluentAssertions;
using Helpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="RespondReactor{TDataIn,TDataOut}"/> class.
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
        var sut = new RespondReactor<int, ResultTestData>(id);

        // Assert
        sut.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsName()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new RespondReactor<int, ResultTestData>(id, "test-name");

        // Assert
        sut.Name.Should().Be("test-name");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnRespond_WhenUnsubscribed_ReturnsEmptyResult()
    {
        // Arrange
        var sut = new RespondReactor<It.IsAnyType, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>());

        sut.OnUnsubscribe();

        // Act
        var actual = sut.OnRespond(It.IsAny<IMessage<It.IsAnyType>>());

        // Assert
        actual.Should().NotBeNull();
        actual.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void OnRespond_WithNullMessage_ThrowsException()
    {
        // Arrange
        var totalActionInvokes = 0;
        var sut = new RespondReactor<int, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            onRespondMsg: _ =>
            {
                totalActionInvokes++;
                return new Mock<IResult<ResultTestData>>().Object;
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
        var message = new Mock<IMessage<int>>();
        var result = new Mock<IResult<ResultTestData>>();

        var sut = new RespondReactor<int, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            onRespondMsg: msg =>
            {
                msg.Should().BeSameAs(message.Object);
                totalActionInvokes++;
                return result.Object;
            });

        // Act
        var actual = sut.OnRespond(message.Object);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeSameAs(result.Object);
        totalActionInvokes.Should().Be(1);
    }

    [Fact]
    public void OnUnsubscribe_WhenInvoked_UnsubscribesReactor()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondReactor<int, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
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

        var sut = new RespondReactor<int, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            onError: _ => totalActionInvokes++);

        sut.OnUnsubscribe();

        // Act
        sut.OnError(It.IsAny<Exception>());

        // Assert
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnError_WhenNotUnsubscribedAndWithNullParam_ThrowsException()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondReactor<int, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
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

        var sut = new RespondReactor<int, ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
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
    [InlineData("test-value", "87e99bdc-a972-427a-90be-f2c07c4f9aef", "test-value - 87e99bdc-a972-427a-90be-f2c07c4f9aef")]
    [InlineData(null, "87e99bdc-a972-427a-90be-f2c07c4f9aef", "87e99bdc-a972-427a-90be-f2c07c4f9aef")]
    [InlineData("", "87e99bdc-a972-427a-90be-f2c07c4f9aef", "87e99bdc-a972-427a-90be-f2c07c4f9aef")]
    public void ToString_WhenInvoked_ReturnsCorrectResult(
        string name,
        string guid,
        string expected)
    {
        // Arrange
        var id = new Guid(guid);

        var sut = new RespondReactor<It.IsAnyType, It.IsAnyType>(id, name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
