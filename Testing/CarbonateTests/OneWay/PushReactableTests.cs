// <copyright file="PushReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests.OneWay;

using Carbonate.Core.OneWay;
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
        var act = () => sut.Push(null, Guid.NewGuid());

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
        var act = () => sut.Push(123, Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable<int>)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void Push_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

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
        sut.Push(123, invokedEventId);

        // Assert
        mockSubA.Received(1).OnReceive(Arg.Any<int>());
        mockSubB.DidNotReceive().OnReceive(Arg.Any<int>());
        mockSubC.Received(1).OnReceive(Arg.Any<int>());
    }

    [Fact]
    public void Push_WhenUnsubscribingInsideOnReceiveSubscriptionAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initSubA = new ReceiveSubscription<int>(id: mainId, _ => { });

        var otherSubA = new ReceiveSubscription<int>(id: otherId, _ => { });
        var otherSubB = new ReceiveSubscription<int>(id: otherId, _ => { });

        const int data = 123;

        var sut = CreateSystemUnderTest();

        var initSubC = new ReceiveSubscription<int>(
            id: mainId,
            onReceive: _ =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initSubA);
        otherUnsubscriberA = sut.Subscribe(otherSubA);
        otherUnsubscriberB = sut.Subscribe(otherSubB);
        sut.Subscribe(initSubC);

        // Act
        var act = () => sut.Push(data, mainId);

        // Assert
        act.Should().NotThrow<Exception>();
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
        sut.Push(123, idA);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushReactable<int> CreateSystemUnderTest() => new ();
}
