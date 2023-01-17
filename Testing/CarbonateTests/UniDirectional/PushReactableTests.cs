// <copyright file="PushReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests.UniDirectional;

using Carbonate.Core.UniDirectional;
using Carbonate.Services;
using Carbonate.UniDirectional;
using Helpers;
using FluentAssertions;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="PushReactable{T}"/> class.
/// </summary>
public class PushReactableTests
{
    private readonly Mock<ISerializerService> mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PushReactableTests"/> class.
    /// </summary>
    public PushReactableTests() => this.mockSerializerService = new Mock<ISerializerService>();

    #region Method Tests
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

        var mockReactorA = new Mock<IReceiveReactor<int>>();
        mockReactorA.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockReactorB = new Mock<IReceiveReactor<int>>();
        mockReactorB.SetupGet(p => p.Id).Returns(notInvokedEventId);

        var mockReactorC = new Mock<IReceiveReactor<int>>();
        mockReactorC.SetupGet(p => p.Id).Returns(invokedEventId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // Act
        sut.Push(123, invokedEventId);

        // Assert
        mockReactorA.Verify(m => m.OnReceive(It.IsAny<int>()), Times.Once);
        mockReactorB.Verify(m => m.OnReceive(It.IsAny<int>()), Times.Never);
        mockReactorC.Verify(m => m.OnReceive(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Push_WhenUnsubscribingInsideOnReceiveReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor<int>(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor<int>(eventId: otherId);
        var otherReactorB = new ReceiveReactor<int>(eventId: otherId);

        const int data = 123;

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveReactor<int>(
            eventId: mainId,
            onReceiveData: _ =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initReactorA);
        otherUnsubscriberA = sut.Subscribe(otherReactorA);
        otherUnsubscriberB = sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

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
        var reactorA = new ReceiveReactor<int>(
            eventId: idA,
            onReceiveData: _ => throw new Exception("test-exception"),
            onError: e =>
            {
                // Assert
                e.Should().BeOfType<Exception>();
                e.Message.Should().Be("test-exception");
            });
        var reactorB = new ReceiveReactor<int>(idB);

        sut.Subscribe(reactorA);
        sut.Subscribe(reactorB);

        // Act
        sut.Push(It.IsAny<int>(), idA);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushReactable<int> CreateSystemUnderTest() => new ();
}
