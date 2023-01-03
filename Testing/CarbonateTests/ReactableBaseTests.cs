// <copyright file="ReactableBaseTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using Carbonate;
using FluentAssertions;
using Helpers.Fakes;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

public class ReactableBaseTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactableBaseTests"/> class.
    /// </summary>
    public ReactableBaseTests()
    {
    }

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
        var mockReactorA = Substitute.For<IReceiveReactor>();
        var mockReactorB = Substitute.For<IReceiveReactor>();

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
    public void Unsubscribe_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Unsubscribe(Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingSomeEvents_UnsubscribesCorrectReactors()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReactor>();
        mockReactorA.Id.Returns(eventToUnsubscribeFrom);

        var mockReactorB = Substitute.For<IReactor>();
        mockReactorB.Id.Returns(eventNotToUnsubscribeFrom);

        var mockReactorC = Substitute.For<IReactor>();
        mockReactorC.Id.Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorC);

        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockReactorA.Received(1).OnUnsubscribe();
        mockReactorB.Received(Quantity.None()).OnUnsubscribe();
        mockReactorC.Received(1).OnUnsubscribe();
        sut.Reactors.Should().HaveCount(1);
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingAllEventsOneAtATime_UnsubscribesCorrectReactors()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(eventToUnsubscribeFrom);
        mockReactorA.Unsubscribed.Returns(true);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(eventToUnsubscribeFrom);
        mockReactorB.Unsubscribed.Returns(true);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);

        sut.Unsubscribe(eventToUnsubscribeFrom);
        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockReactorA.Received(1).OnUnsubscribe();
        mockReactorB.Received(1).OnUnsubscribe();
        sut.Reactors.Should().BeEmpty();
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingInsideOnUnsubscribeReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveReactor(
            eventId: mainId,
            onUnsubscribe: () =>
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
            .WithMessage($"{nameof(PushReactable)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void UnsubscribeAll_WhenInvoked_UnsubscribesFromAll()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(eventToUnsubscribeFrom);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(eventNotToUnsubscribeFrom);

        var mockReactorC = Substitute.For<IReceiveReactor>();
        mockReactorC.Id.Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorC);
        sut.Subscribe(mockReactorB);
        sut.Subscribe(mockReactorA);

        sut.UnsubscribeAll();

        // Assert
        mockReactorA.Received(1).OnUnsubscribe();
        mockReactorB.Received(1).OnUnsubscribe();
        mockReactorC.Received(1).OnUnsubscribe();
        sut.Reactors.Should().BeEmpty();
    }

    [Fact]
    public void UnsubscribeAll_WhenUnsubscribingInsideOnUnsubscribeReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initReactorA = new ReceiveReactor(
            eventId: mainId);

        var otherReactorA = new ReceiveReactor(eventId: otherId);
        var otherReactorB = new ReceiveReactor(eventId: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveReactor(
            eventId: mainId,
            onUnsubscribe: () =>
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

        var mockReactorA = Substitute.For<IReceiveReactor>();
        mockReactorA.Id.Returns(eventIdA);

        var mockReactorB = Substitute.For<IReceiveReactor>();
        mockReactorB.Id.Returns(eventIdB);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA);
        sut.Subscribe(mockReactorB);

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
        mockReactorA.Received(1).OnUnsubscribe();
        mockReactorB.Received(1).OnUnsubscribe();

        sut.Reactors.Should().BeEmpty();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ReactableBaseFake CreateSystemUnderTest() => new ();
}
