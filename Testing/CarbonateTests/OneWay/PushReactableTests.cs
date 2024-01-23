// <copyright file="PushReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests.OneWay;

using Carbonate.Core.OneWay;
using Carbonate.Exceptions;
using Carbonate.OneWay;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="PushReactable{T}"/> class.
/// </summary>
public class PushReactableTests
{
    #region Method Tests
    [Fact]
    public void Push_WhenDataParamIsNull_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<object>();

        // Act
        var act = () => sut.Push(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'data')");
    }

    [Fact]
    public void Push_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Push(Guid.Empty, 123);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable<int>)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void Push_WhenUnsubscribingWhileProcessingNotifications_ThrowsException()
    {
        // Arrange
        var id = new Guid("9c252828-9c1a-413e-ab60-1547a09dae0e");
        const string subName = "test-subscription";
        var expectedMsg = "The send notification process is currently in progress.";
        expectedMsg += $"\nThe subscription '{subName}' with id '{id}' could not be unsubscribed.";

        IDisposable? unsubscriber = null;
        var mockSubscription = Substitute.For<IReceiveSubscription<int>>();
        mockSubscription.Id.Returns(id);
        mockSubscription.Name.Returns(subName);
        mockSubscription.When(x => x.OnReceive(123))
            .Do(_ =>
            {
                unsubscriber.Dispose();
            });

        var sut = CreateSystemUnderTest();
        unsubscriber = sut.Subscribe(mockSubscription);

        // Act
        var act = () => sut.Push(id, 123);

        // Assert
        act.Should().Throw<NotificationException>().WithMessage(expectedMsg);
    }

    [Fact]
    public void Push_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = new Guid("0cf574c1-f62c-4b89-be0f-19e6a6f8de35");
        var notInvokedEventId = new Guid("1eba03c2-a911-45d0-aa62-11ac7eb17a0d");

        var mockSubA = Substitute.For<IReceiveSubscription<int>>();
        mockSubA.Id.Returns(invokedEventId);

        var mockSubB = Substitute.For<IReceiveSubscription<int>>();
        mockSubB.Id.Returns(notInvokedEventId);

        var mockSubC = Substitute.For<IReceiveSubscription<int>>();
        mockSubC.Id.Returns(invokedEventId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);
        sut.Subscribe(mockSubC);

        // Act
        sut.Push(invokedEventId, 123);

        // Assert
        mockSubA.Received(1).OnReceive(123);
        mockSubB.DidNotReceive().OnReceive(123);
        mockSubC.Received(1).OnReceive(123);
    }

    [Fact]
    public void Push_WhenSubscriptionThrowsException_InvokesSubscriptionOnError()
    {
        // Arrange
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();

        var sut = CreateSystemUnderTest();
        var subA = new ReceiveSubscription<int>(
            id: idA,
            onReceive: _ => throw new Exception("test-exception"),
            onError: e =>
            {
                // Assert
                e.Should().BeOfType<Exception>();
                e.Message.Should().Be("test-exception");
            });
        var subB = new ReceiveSubscription<int>(idB, _ => { });

        sut.Subscribe(subA);
        sut.Subscribe(subB);

        // Act
        sut.Push(idA, 123);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushReactable<int> CreateSystemUnderTest() => new ();
}
