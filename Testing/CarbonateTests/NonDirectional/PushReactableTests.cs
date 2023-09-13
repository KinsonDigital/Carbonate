// <copyright file="PushReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests.NonDirectional;

using Carbonate.Core.NonDirectional;
using Carbonate.NonDirectional;
using FluentAssertions;
using Moq;
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
    public void Push_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = new Mock<IReceiveSubscription>();
        mockReactorA.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockReactorB = new Mock<IReceiveSubscription>();
        mockReactorB.SetupGet(p => p.Id).Returns(notInvokedEventId);

        var mockReactorC = new Mock<IReceiveSubscription>();
        mockReactorC.SetupGet(p => p.Id).Returns(invokedEventId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // Act
        sut.Push(invokedEventId);

        // Assert
        mockReactorA.Verify(m => m.OnReceive(), Times.Once);
        mockReactorB.Verify(m => m.OnReceive(), Times.Never);
        mockReactorC.Verify(m => m.OnReceive(), Times.Once);
    }

    [Fact]
    public void Push_WhenUnsubscribingInsideOnReceiveReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveSubscription(
            id: mainId);

        var otherReactorA = new ReceiveSubscription(id: otherId);
        var otherReactorB = new ReceiveSubscription(id: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveSubscription(
            id: mainId,
            onReceive: () =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initReactorA);
        otherUnsubscriberA = sut.Subscribe(otherReactorA);
        otherUnsubscriberB = sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

        // Act
        var act = () => sut.Push(mainId);

        // Assert
        act.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Push_WhenExceptionOccursInOnReceiveSubscription_InvokesOnErrorForReactor()
    {
        // Arrange
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();

        var reactorA = new ReceiveSubscription(
            id: idA,
            onReceive: () => throw new Exception("test-exception"),
            onError: e =>
            {
                e.Should().BeOfType<Exception>();
                e.Message.Should().Be("test-exception");
            });

        var reactorB = new ReceiveSubscription(id: idB);

        var sut = CreateSystemUnderTest();

        sut.Subscribe(reactorA);
        sut.Subscribe(reactorB);

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
