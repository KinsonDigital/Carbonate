// <copyright file="ReactableTests.cs" company="KinsonDigital">
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
/// Tests the <see cref="Reactable"/> class.
/// </summary>
public class ReactableTests
{
    private readonly ISerializerService mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactableTests"/> class.
    /// </summary>
    public ReactableTests() => this.mockSerializerService = Substitute.For<ISerializerService>();

    #region Prop Tests
    [Fact]
    public void EventIds_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var eventIdA = Guid.NewGuid();
        var eventIdB = Guid.NewGuid();
        var eventIdC = eventIdA;

        var expected = new[] { eventIdA, eventIdB };

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(eventIdA);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(eventIdB);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.EventId.Returns(eventIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        var actual = sut.EventIds;

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
            .WithMessage($"{nameof(Reactable)} disposed.{Environment.NewLine}Object name: 'Reactable'.");
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
            .WithMessage($"{nameof(Reactable)} disposed.{Environment.NewLine}Object name: 'Reactable'.");
    }

    [Fact]
    public void Subscribe_WithNullReactor_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var act = () => sut.Subscribe(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'reactor')");
    }

    [Fact]
    public void Subscribe_WhenInvoked_ReactorsPropReturnsReactors()
    {
        // Arrange
        var mockReactorA = Substitute.For<IReactor>();
        var mockReactorB = Substitute.For<IReactor>();

        var expected = new[] { mockReactorA, mockReactorB };

        var sut = CreateSystemUnderTest();

        // Act
        var reactorUnsubscriberA = sut.Subscribe(mockReactorA);
        var reactorUnsubscriberB = sut.Subscribe(mockReactorB);

        var actual = sut.Reactors;

        // Assert
        actual.Should().BeEquivalentTo(expected);
        actual[0].Should().BeSameAs(mockReactorA);
        actual[1].Should().BeSameAs(mockReactorB);
        reactorUnsubscriberA.Should().NotBeNull();
        reactorUnsubscriberB.Should().NotBeNull();
    }

    [Fact]
    public void Push_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(notInvokedEventId);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.EventId.Returns(invokedEventId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        sut.Push(invokedEventId);

        // Assert
        mockReactorA.Received(1).OnNext();
        mockReactorB.Received(Quantity.None()).OnNext();
        mockReactorC.Received(1).OnNext();
    }

    [Fact]
    public void Push_WhenUnsubscribingInsideOnNexReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new Reactor(
            eventId: mainId);

        var otherReactorA = new Reactor(eventId: otherId);
        var otherReactorB = new Reactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new Reactor(
            eventId: mainId,
            onNext: () =>
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
            .WithMessage($"{nameof(Reactable)} disposed.{Environment.NewLine}Object name: 'Reactable'.");
    }

    [Fact]
    public void PushData_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Returns("test-json-data");
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(notInvokedEventId);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.EventId.Returns(invokedEventId);

        var testData = default(TestData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        sut.PushData(testData, invokedEventId);

        // Assert
        this.mockSerializerService.Received(1).Serialize(testData);
        mockReactorA.Received(1).OnNext(Arg.Any<JsonMessage>());
        mockReactorB.Received(Quantity.None()).OnNext(Arg.Any<JsonMessage>());
        mockReactorC.Received(1).OnNext(Arg.Any<JsonMessage>());
    }

    [Fact]
    public void PushData_WithSerializationError_NotifiesSubscribersOfError()
    {
        // Arrange
        var expected = new JsonException("serial-error");
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Throws(expected);

        var invokedEventId = Guid.NewGuid();
        var otherEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(otherEventId);

        var testData = default(TestData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);

        // Act
        sut.PushData(testData, invokedEventId);

        // Assert
        mockReactorA.Received(Quantity.None()).OnNext(Arg.Any<JsonMessage>());
        mockReactorB.Received(Quantity.None()).OnNext(Arg.Any<JsonMessage>());

        mockReactorA.Received(1).OnError(expected);
        mockReactorB.Received(Quantity.None()).OnError(expected);
    }

    [Fact]
    public void PushData_WhenUnsubscribingInsideOnNextReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new Reactor(
            eventId: mainId);

        var otherReactorA = new Reactor(eventId: otherId);
        var otherReactorB = new Reactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var testData = new TestData();

        var initReactorC = new Reactor(
            eventId: mainId,
            onNextMsg: _ =>
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

        var initReactorA = new Reactor(
            eventId: mainId);

        var otherReactorA = new Reactor(eventId: otherId);
        var otherReactorB = new Reactor(eventId: otherId);

        var testData = new TestData();

        var sut = CreateSystemUnderTest();

        var initReactorC = new Reactor(
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
            .WithMessage($"{nameof(Reactable)} disposed.{Environment.NewLine}Object name: 'Reactable'.");
    }

    [Fact]
    public void PushMessage_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        var invokedEventId = Guid.NewGuid();
        var notInvokedEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(notInvokedEventId);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.EventId.Returns(invokedEventId);

        var mockMessage = Substitute.For<IMessage>();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        // Act
        sut.PushMessage(mockMessage, invokedEventId);

        // Assert
        mockReactorA.Received(1).OnNext(Arg.Any<IMessage>());
        mockReactorB.Received(Quantity.None()).OnNext(Arg.Any<IMessage>());
        mockReactorC.Received(1).OnNext(Arg.Any<IMessage>());
    }

    [Fact]
    public void PushMessage_WhenUnsubscribingInsideOnNextReactorAction_DoesNotThrowException()
    {
        // Arrange
        this.mockSerializerService.Serialize(Arg.Any<TestData>()).Returns("test-data");
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        IDisposable? otherUnsubscriberA = null;
        IDisposable? otherUnsubscriberB = null;

        var initReactorA = new Reactor(
            eventId: mainId);

        var otherReactorA = new Reactor(eventId: otherId);
        var otherReactorB = new Reactor(eventId: otherId);

        var mockMessage = Substitute.For<IMessage>();

        var sut = CreateSystemUnderTest();

        var initReactorC = new Reactor(
            eventId: mainId,
            onNextMsg: _ =>
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

    [Fact]
    public void Unsubscribe_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Unsubscribe(Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(Reactable)} disposed.{Environment.NewLine}Object name: 'Reactable'.");
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingSomeEvents_UnsubscribesCorrectReactors()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(eventToUnsubscribeFrom);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(eventNotToUnsubscribeFrom);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.EventId.Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockReactorA.Received(1).OnComplete();
        mockReactorB.Received(Quantity.None()).OnComplete();
        mockReactorC.Received(1).OnComplete();
        sut.Reactors.Should().HaveCount(1);
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingAllEventsOneAtATime_UnsubscribesCorrectReactors()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(eventToUnsubscribeFrom);
        mockReactorA.Unsubscribed.Returns(true);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(eventToUnsubscribeFrom);
        mockReactorB.Unsubscribed.Returns(true);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);

        sut.Unsubscribe(eventToUnsubscribeFrom);
        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockReactorA.Received(1).OnComplete();
        mockReactorB.Received(1).OnComplete();
        sut.Reactors.Should().BeEmpty();
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingInsideOnCompleteReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initReactorA = new Reactor(
            eventId: mainId);

        var otherReactorA = new Reactor(eventId: otherId);
        var otherReactorB = new Reactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new Reactor(
            eventId: mainId,
            onCompleted: () =>
            {
                sut.Unsubscribe(otherId);
            });

        sut.Subscribe(initReactorA);
        sut.Subscribe(otherReactorA);
        sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

        // Act
        var act = () => sut.Unsubscribe(mainId);

        // Assert
        act.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void UnsubscribeAll_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.UnsubscribeAll();

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(Reactable)} disposed.{Environment.NewLine}Object name: 'Reactable'.");
    }

    [Fact]
    public void UnsubscribeAll_WhenInvoked_UnsubscribesFromAll()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(eventToUnsubscribeFrom);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(eventNotToUnsubscribeFrom);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.EventId.Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorC);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorA);

        sut.UnsubscribeAll();

        // Assert
        mockReactorA.Received(1).OnComplete();
        mockReactorB.Received(1).OnComplete();
        mockReactorC.Received(1).OnComplete();
        sut.Reactors.Should().BeEmpty();
    }

    [Fact]
    public void UnsubscribeAll_WhenUnsubscribingInsideOnCompleteReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initReactorA = new Reactor(
            eventId: mainId);

        var otherReactorA = new Reactor(eventId: otherId);
        var otherReactorB = new Reactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new Reactor(
            eventId: mainId,
            onCompleted: () =>
            {
                sut.Unsubscribe(otherId);
            });

        sut.Subscribe(initReactorA);
        sut.Subscribe(otherReactorA);
        sut.Subscribe(otherReactorB);
        sut.Subscribe(initReactorC);

        // Act
        var act = () => sut.UnsubscribeAll();

        // Assert
        act.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Dispose_WhenInvoked_DisposesOfReactable()
    {
        // Arrange
        var eventIdA = Guid.NewGuid();
        var eventIdB = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(eventIdA);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(eventIdB);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
        mockReactorA.Received(1).OnComplete();
        mockReactorB.Received(1).OnComplete();

        sut.Reactors.Should().BeEmpty();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="Reactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private Reactable CreateSystemUnderTest() => new (this.mockSerializerService);
}
