// <copyright file="ReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

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
    private readonly ISerializer mockSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactableTests"/> class.
    /// </summary>
    public ReactableTests() => this.mockSerializer = Substitute.For<ISerializer>();

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
    public void PushData_WhenInvoking_NotifiesCorrectSubscriptionsThatMatchEventId()
    {
        // Arrange
        this.mockSerializer.Serialize(Arg.Any<TestData>()).Returns("test-json-data");
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
        this.mockSerializer.Received(1).Serialize(testData);
        mockReactorA.Received(1).OnNext(Arg.Any<JsonMessage>());
        mockReactorB.Received(Quantity.None()).OnNext(Arg.Any<JsonMessage>());
        mockReactorC.Received(1).OnNext(Arg.Any<JsonMessage>());
    }

    [Fact]
    public void PushData_WithSerializationError_NotifiesSubscribersOfError()
    {
        // Arrange
        var expected = new Exception("serial-error");
        this.mockSerializer.Serialize(Arg.Any<TestData>()).Throws(expected);

        var invokedEventId = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.EventId.Returns(invokedEventId);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.EventId.Returns(invokedEventId);

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
        mockReactorB.Received(1).OnError(expected);
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
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingAllEvents_UnsubscribesCorrectReactors()
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
    private Reactable CreateSystemUnderTest()
        => new (this.mockSerializer);
}
