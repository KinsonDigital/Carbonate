// <copyright file="RespondReactorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.UniDirectional;

using Carbonate.Core;
using Carbonate.UniDirectional;
using FluentAssertions;
using Helpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="RespondReactor{TDataOut}"/> class.
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
        var sut = new RespondReactor<ResultTestData>(id);

        // Assert
        sut.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsName()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new RespondReactor<ResultTestData>(id, "test-name");

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
        var sut = new RespondReactor<ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return It.IsAny<IResult<ResultTestData>>();
            });

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
        var sut = new RespondReactor<ResultTestData>(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            onRespond: () =>
            {
                totalActionInvokes++;
                return new Mock<IResult<ResultTestData>>().Object;
            });

        // Act
        _ = sut.OnRespond();

        // Assert
        totalActionInvokes.Should().Be(1);
    }

    [Fact]
    public void OnUnsubscribe_WhenInvoked_UnsubscribesReactor()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new RespondReactor<ResultTestData>(
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

        var sut = new RespondReactor<ResultTestData>(
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

        var sut = new RespondReactor<ResultTestData>(
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

        var sut = new RespondReactor<ResultTestData>(
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

        var sut = new RespondReactor<It.IsAnyType>(
            respondId: id,
            name: name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
