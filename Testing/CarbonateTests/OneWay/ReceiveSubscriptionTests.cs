// <copyright file="ReceiveSubscriptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.OneWay;

using Carbonate.OneWay;
using Shouldly;
using Xunit;

/// <summary>
/// Tests the <see cref="ReceiveSubscription{TIn}"/> class.
/// </summary>
public class ReceiveSubscriptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsEventId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var sut = new ReceiveSubscription<int>(guid, _ => { });
        var actual = sut.Id;

        // Assert
        actual.ShouldBe(guid);
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

        var sut = new ReceiveSubscription<int>(Guid.NewGuid(), onReceive: OnReceive);

        // Act
        sut.OnReceive(data);

        // Assert
        onReceiveInvoked.ShouldBeTrue();
    }

    [Fact]
    public void OnReceive_WhenSendingDataAndNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnReceive(int incomingData) => onReceiveInvoked = true;

        const int data = 123;

        var sut = new ReceiveSubscription<int>(Guid.NewGuid(), onReceive: OnReceive);

        sut.OnUnsubscribe();

        // Act
        sut.OnReceive(data);

        // Assert
        onReceiveInvoked.ShouldBeFalse();
    }

    [Fact]
    public void OnReceive_WhenSendingNullData_ThrowsException()
    {
        // Arrange
        var sut = new ReceiveSubscription<object>(Guid.NewGuid(), _ => { });

        // Act
        Action act = () => sut.OnReceive(null);

        // Assert
        Should.Throw<ArgumentNullException>(act)
            .Message.ShouldBe("The parameter must not be null. (Parameter 'data')");
    }

    [Theory]
    [InlineData(null, "5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    [InlineData("", "5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    [InlineData("test-value", "test-value - 5739afd9-be4c-4402-a12d-6bcde35cc8c3")]
    public void ToString_WhenInvoked_ReturnsCorrectResult(string? name, string expected)
    {
        // Arrange
        var id = new Guid("5739afd9-be4c-4402-a12d-6bcde35cc8c3");

        var sut = new ReceiveSubscription<int>(
            id: id,
            onReceive: _ => { },
            name: name);

        // Act
        var actual = sut.ToString();

        // Assert
        actual.ShouldBe(expected);
    }
    #endregion
}
