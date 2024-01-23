// <copyright file="PushReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests.NonDirectional;

using Carbonate.Core.NonDirectional;
using Carbonate.Exceptions;
using Carbonate.NonDirectional;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class PushReactableTests
{
    #region Method Tests
    [Fact]
    public void Push_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Push(Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void Push_WhenUnsubscribingWhileProcessingNotifications_ThrowsException()
    {
        // Arrange
        var id = new Guid("163b36a5-e043-480b-a8f9-53822316bd4b");
        const string subName = "test-subscription";
        var expectedMsg = "The send notification process is currently in progress.";
        expectedMsg += $"\nThe subscription '{subName}' with id '{id}' could not be unsubscribed.";

        IDisposable? unsubscriber = null;
        var mockSubscription = Substitute.For<IReceiveSubscription>();
        mockSubscription.Id.Returns(id);
        mockSubscription.Name.Returns(subName);
        mockSubscription.When(x => x.OnReceive())
            .Do(_ =>
            {
                unsubscriber.Dispose();
            });

        var sut = CreateSystemUnderTest();
        unsubscriber = sut.Subscribe(mockSubscription);

        // Act
        var act = () => sut.Push(id);

        // Assert
        act.Should().Throw<NotificationException>().WithMessage(expectedMsg);
    }

    [Fact]
    public void Push_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockSubA = Substitute.For<IReceiveSubscription>();
        mockSubA.Id.Returns(invokedEventId);

        var mockSubB = Substitute.For<IReceiveSubscription>();
        mockSubB.Id.Returns(notInvokedEventId);

        var mockSubC = Substitute.For<IReceiveSubscription>();
        mockSubC.Id.Returns(invokedEventId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);
        sut.Subscribe(mockSubC);

        // Act
        sut.Push(invokedEventId);

        // Assert
        mockSubA.Received(1).OnReceive();
        mockSubB.DidNotReceive().OnReceive();
        mockSubC.Received(1).OnReceive();
    }

    [Fact]
    public void Push_WhenUnsubscribingInsideOnReceiveSubscriptionAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initSubA = new ReceiveSubscription(id: mainId, () => { });

        var otherSubA = new ReceiveSubscription(id: otherId, () => { });
        var otherSubB = new ReceiveSubscription(id: otherId, () => { });

        var sut = CreateSystemUnderTest();

        var initSubC = new ReceiveSubscription(
            id: mainId,
            onReceive: () =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initSubA);
        otherUnsubscriberA = sut.Subscribe(otherSubA);
        otherUnsubscriberB = sut.Subscribe(otherSubB);
        sut.Subscribe(initSubC);

        // Act
        var act = () => sut.Push(mainId);

        // Assert
        act.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Push_WhenExceptionOccursInOnReceiveSubscription_InvokesOnErrorForSubscription()
    {
        // Arrange
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();

        var subA = new ReceiveSubscription(
            id: idA,
            onReceive: () => throw new Exception("test-exception"),
            onError: e =>
            {
                e.Should().BeOfType<Exception>();
                e.Message.Should().Be("test-exception");
            });

        var subB = new ReceiveSubscription(id: idB, () => { });

        var sut = CreateSystemUnderTest();

        sut.Subscribe(subA);
        sut.Subscribe(subB);

        // Act
        var act = () => sut.Push(idA);

        // Assert
        act.Should().NotThrow<ArgumentOutOfRangeException>();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushReactable CreateSystemUnderTest() => new ();
}
