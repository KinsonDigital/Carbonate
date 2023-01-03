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
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using Xunit;

/// <summary>
/// Tests the <see cref="PushReactable"/> class.
/// </summary>
public class PushReactableTests
{
    private readonly ISerializerService mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PushReactableTests"/> class.
    /// </summary>
    public PushReactableTests() => this.mockSerializerService = Substitute.For<ISerializerService>();

    #region Prop Tests
    [Fact]
    public void EventIds_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var eventIdA = Guid.NewGuid();
        var eventIdB = Guid.NewGuid();
        var eventIdC = eventIdA;

        var expected = new[] { eventIdA, eventIdB };

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(eventIdA);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(eventIdB);

        var mockReactorC = Substitute.For<IReceiveReactor>();
        mockReactorC.Id.Returns(eventIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

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

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(notInvokedEventId);

        var mockReactorC = Substitute.For<IReceiveReactor>();
        mockReactorC.Id.Returns(invokedEventId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        sut.Push(invokedEventId);

        // Assert
        mockReactorA.Received(1).OnReceive();
        mockReactorB.Received(Quantity.None()).OnReceive();
        mockReactorC.Received(1).OnReceive();
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
        var act = () => sut.PushData(new TestData(), Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void PushData_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Returns("test-json-data");
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(notInvokedEventId);

        var mockReactorC = Substitute.For<IReceiveReactor>();
        mockReactorC.Id.Returns(invokedEventId);

        var testData = default(TestData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        sut.PushData(testData, invokedEventId);

        // Assert
        this.mockSerializerService.Received(1).Serialize(testData);
        mockReactorA.Received(1).OnReceive(Arg.Any<JsonMessage>());
        mockReactorB.Received(Quantity.None()).OnReceive(Arg.Any<JsonMessage>());
        mockReactorC.Received(1).OnReceive(Arg.Any<JsonMessage>());
    }

    [Fact]
    public void PushData_WithSerializationError_NotifiesSubscribersOfError()
    {
        // Arrange
        var expected = new JsonException("serial-error");
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Throws(expected);

        var invokedEventId = Guid.NewGuid();
        var otherEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(otherEventId);

        var testData = default(TestData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);

        // Act
        sut.PushData(testData, invokedEventId);

        // Assert
        mockReactorA.Received(Quantity.None()).OnReceive(Arg.Any<JsonMessage>());
        mockReactorB.Received(Quantity.None()).OnReceive(Arg.Any<JsonMessage>());

        mockReactorA.Received(1).OnError(expected);
        mockReactorB.Received(Quantity.None()).OnError(expected);
    }

    [Fact]
    public void PushData_WhenUnsubscribingInsideOnReceiveReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var testData = new TestData();

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
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Throws(expected);

        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var testData = new TestData();

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
        var act = () => sut.PushMessage(Substitute.For<IMessage>(), Guid.Empty);

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

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(notInvokedEventId);

        var mockReactorC = Substitute.For<IReceiveReactor>();
        mockReactorC.Id.Returns(invokedEventId);

        var mockMessage = Substitute.For<IMessage>();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        sut.PushMessage(mockMessage, invokedEventId);

        // Assert
        mockReactorA.Received(1).OnReceive(Arg.Any<IMessage>());
        mockReactorB.Received(Quantity.None()).OnReceive(Arg.Any<IMessage>());
        mockReactorC.Received(1).OnReceive(Arg.Any<IMessage>());
    }

    [Fact]
    public void PushMessage_WhenUnsubscribingInsideOnReceiveReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var mockMessage = Substitute.For<IMessage>();

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
        var act = () => sut.PushMessage(mockMessage, mainId);

        // Assert
        act.Should().NotThrow<Exception>();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private PushReactable CreateSystemUnderTest() => new (this.mockSerializerService);
}
