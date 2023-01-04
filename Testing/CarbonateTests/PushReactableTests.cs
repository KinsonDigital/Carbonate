// <copyright file="PushReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests;

using System.Text.Json;
using Carbonate;
using Carbonate.Services;
using Helpers;
using FluentAssertions;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="PushReactable"/> class.
/// </summary>
public class PushReactableTests
{
    private readonly Mock<ISerializerService> mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PushReactableTests"/> class.
    /// </summary>
    public PushReactableTests() => this.mockSerializerService = new Mock<ISerializerService>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullSerializerServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new PushReactable(null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'serializerService')");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void EventIds_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var eventIdA = Guid.NewGuid();
        var eventIdB = Guid.NewGuid();
        var eventIdC = eventIdA;

        var expected = new[] { eventIdA, eventIdB };

        var mockReactorA = new Mock<IReceiveReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(eventIdA);

        var mockReactorB = new Mock<IReceiveReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(eventIdB);

        var mockReactorC = new Mock<IReceiveReactor>();
        mockReactorC.SetupGet(p => p.Id).Returns(eventIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // Act
        var actual = sut.SubscriptionIds;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    #endregion

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
    public void Subscribe_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Subscribe(null);

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

        var mockReactorA = new Mock<IReceiveReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockReactorB = new Mock<IReceiveReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(notInvokedEventId);

        var mockReactorC = new Mock<IReceiveReactor>();
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
    public void Push_WhenUnsubscribingInsideOnNexReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveReactor(
            eventId: mainId,
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
    public void PushData_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.PushData(new PullTestData(), Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void PushData_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns("test-json-data");
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = new Mock<IReceiveReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockReactorB = new Mock<IReceiveReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(notInvokedEventId);

        var mockReactorC = new Mock<IReceiveReactor>();
        mockReactorC.SetupGet(p => p.Id).Returns(invokedEventId);

        var testData = default(PullTestData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // Act
        sut.PushData<PullTestData>(testData, invokedEventId);

        // Assert
        this.mockSerializerService.Verify(m => m.Serialize(testData), Times.Once);
        mockReactorA.Verify(m => m.OnReceive(It.IsAny<JsonMessage>()), Times.Once);
        mockReactorB.Verify(m => m.OnReceive(It.IsAny<JsonMessage>()), Times.Never);
        mockReactorC.Verify(m => m.OnReceive(It.IsAny<JsonMessage>()), Times.Once);
    }

    [Fact]
    public void PushData_WithSerializationError_NotifiesSubscribersOfError()
    {
        // Arrange
        var expected = new JsonException("serial-error");
        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Throws(expected);

        var invokedEventId = Guid.NewGuid();
        var otherEventId = Guid.NewGuid();

        var mockReactorA = new Mock<IReceiveReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockReactorB = new Mock<IReceiveReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(otherEventId);

        var testData = default(PullTestData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        sut.PushData(testData, invokedEventId);

        // Assert
        mockReactorA.Verify(m => m.OnReceive(It.IsAny<JsonMessage>()), Times.Never);
        mockReactorB.Verify(m => m.OnReceive(It.IsAny<JsonMessage>()), Times.Never);

        mockReactorA.Verify(m =>  m.OnError(expected), Times.Once);
        mockReactorB.Verify(m => m.OnError(expected), Times.Never);
    }

    [Fact]
    public void PushData_WhenUnsubscribingInsideOnReceiveReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var testData = new PullTestData();

        var initReactorC = new ReceiveReactor(
            eventId: mainId,
            onReceiveMsg: _ =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initReactorA);
        otherUnsubscriberA = sut.Subscribe(otherReactorA);
        otherUnsubscriberB = sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

        // Act
        var act = () => sut.PushData(testData, mainId);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void PushData_WhenUnsubscribingInsideOnErrorReactorAction_DoesNotThrowException()
    {
        // Arrange
        var expected = new JsonException("serial-error");
        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Throws(expected);

        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var testData = new PullTestData();

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveReactor(
            eventId: mainId,
            onError: _ =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initReactorA);
        otherUnsubscriberA = sut.Subscribe(otherReactorA);
        otherUnsubscriberB = sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

        // Act
        var act = () => sut.PushData(testData, mainId);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void PushMessage_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.PushMessage(new Mock<IMessage>().Object, Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void PushMessage_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = new Mock<IReceiveReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockReactorB = new Mock<IReceiveReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(notInvokedEventId);

        var mockReactorC = new Mock<IReceiveReactor>();
        mockReactorC.SetupGet(p => p.Id).Returns(invokedEventId);

        var mockMessage = new Mock<IMessage>();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // Act
        sut.PushMessage(mockMessage.Object, invokedEventId);

        // Assert
        mockReactorA.Verify(m => m.OnReceive(It.IsAny<IMessage>()), Times.Once);
        mockReactorB.Verify(m => m.OnReceive(It.IsAny<IMessage>()), Times.Never);
        mockReactorC.Verify(m => m.OnReceive(It.IsAny<IMessage>()), Times.Once);
    }

    [Fact]
    public void PushMessage_WhenUnsubscribingInsideOnReceiveReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var mockMessage = new Mock<IMessage>();

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveReactor(
            eventId: mainId,
            onReceiveMsg: _ =>
            {
                otherUnsubscriberA?.Dispose();
                otherUnsubscriberB?.Dispose();
            });

        sut.Subscribe(initReactorA);
        otherUnsubscriberA = sut.Subscribe(otherReactorA);
        otherUnsubscriberB = sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

        // Act
        var act = () => sut.PushMessage(mockMessage.Object, mainId);

        // Assert
        act.Should().NotThrow<Exception>();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private PushReactable CreateSystemUnderTest() => new (this.mockSerializerService.Object);
}
