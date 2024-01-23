// <copyright file="ReceiveRespondSubscriptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.TwoWay;

using Carbonate.TwoWay;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests the <see cref="ReceiveRespondSubscription{TIn,TOut}"/> class.
/// </summary>
public class ReceiveRespondSubscriptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsIdProperty()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new ReceiveRespondSubscription<int, string>(id, _ => string.Empty);

        // Assert
        sut.Id.Should().Be(id);
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsName()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var sut = new ReceiveRespondSubscription<int, string>(id, _ => "value", "test-name");

        // Assert
        sut.Name.Should().Be("test-name");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnRespond_WhenUnsubscribed_ReturnsCorrectDefaultResult()
    {
        // Arrange
        var sut = new ReceiveRespondSubscription<int, int>(Guid.NewGuid(),
            onReceiveRespond: _ => 456);
        sut.OnUnsubscribe();

        // Act
        var actual = sut.OnRespond(123);

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    public void OnRespond_WhenInvokingWithNullData_ThrowsException()
    {
        // Arrange
        var sut = new ReceiveRespondSubscription<int?, object>(Guid.NewGuid(), _ => new object());

        // Act
        var act = () => sut.OnRespond(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'data')");
    }

    [Fact]
    public void OnRespond_WhenOnRespondDataIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var obj = new object();
        var sut = new ReceiveRespondSubscription<int, object>(Guid.NewGuid(),
            onReceiveRespond: _ => obj);

        // Act
        var actual = sut.OnRespond(123);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeSameAs(obj);
    }

    [Fact]
    public void OnUnsubscribe_WhenInvoked_UnsubscribesSubscription()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new ReceiveRespondSubscription<int, string>(
            Guid.NewGuid(),
            onReceiveRespond: _ => string.Empty,
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

        var sut = new ReceiveRespondSubscription<int, string>(
            Guid.NewGuid(),
            onReceiveRespond: _ => string.Empty,
            onError: _ => totalActionInvokes++);

        sut.OnUnsubscribe();

        // Act
        sut.OnError(new Exception());

        // Assert
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnError_WhenNotUnsubscribedAndWithNullParam_ThrowsException()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new ReceiveRespondSubscription<int, string>(
            Guid.NewGuid(),
            onReceiveRespond: _ => string.Empty,
            onError: _ => totalActionInvokes++);

        // Act
        var act = () => sut.OnError(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'error')");
        totalActionInvokes.Should().Be(0);
    }

    [Fact]
    public void OnError_WhenNotUnsubscribed_InvokesOnErrorAction()
    {
        // Arrange
        var totalActionInvokes = 0;

        var sut = new ReceiveRespondSubscription<int, string>(
            Guid.NewGuid(),
            onReceiveRespond: _ => string.Empty,
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
        string? name,
        string guid,
        string expected)
    {
        // Arrange
        var id = new Guid(guid);

        var sut = new ReceiveRespondSubscription<int, int>(id, _ => 123, name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
