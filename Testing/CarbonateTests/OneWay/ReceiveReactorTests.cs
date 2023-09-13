// <copyright file="ReceiveReactorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.OneWay;

using Carbonate.OneWay;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests the <see cref="ReceiveReactor{T}"/> class.
/// </summary>
public class ReceiveReactorTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsEventId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var sut = new ReceiveReactor<int>(guid);
        var actual = sut.Id;

        // Assert
        actual.Should().Be(guid);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnReceive_WhenSendingDataAndSubscribed_InvokesAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive(int incomingData) => onReceiveInvoked = true;

        const int data = 123;

        var sut = new ReceiveReactor<int>(Guid.NewGuid(), onReceive: OnReceive);

        // Act
        sut.OnReceive(data);

        // Assert
        onReceiveInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnReceive_WhenSendingDataAndNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive(int incomingData) => onReceiveInvoked = true;

        const int data = 123;

        var sut = new ReceiveReactor<int>(Guid.NewGuid(), onReceive: OnReceive);

        sut.OnUnsubscribe();

        // Act
        sut.OnReceive(data);

        // Assert
        onReceiveInvoked.Should().BeFalse();
    }

    [Fact]
    public void OnReceive_WhenSendingNullData_ThrowsException()
    {
        // Arrange
        var sut = new ReceiveReactor<object>(Guid.NewGuid());

        // Act
        var act = () => sut.OnReceive(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'data')");
    }

    [Theory]
    [InlineData(null, "5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    [InlineData("", "5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    [InlineData("test-value", "test-value - 5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    public void ToString_WhenInvoked_ReturnsCorrectResult(string name, string expected)
    {
        // Arrange
        var id = new Guid("5739afd9-be4c-4402-a12d-6bcde35cc8c3");

        var sut = new ReceiveReactor<int>(
            eventId: id,
            name: name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(expected);
    }
    #endregion
}
