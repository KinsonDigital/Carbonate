// <copyright file="ReactableBaseTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using Carbonate.Core;
using Carbonate.Core.OneWay;
using Carbonate.OneWay;
using FluentAssertions;
using Helpers.Fakes;
using Moq;
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

        var mockReactorA = new Mock<IReceiveSubscription<int>>();
        mockReactorA.SetupGet(p => p.Id).Returns(eventIdA);

        var mockReactorB = new Mock<IReceiveSubscription<int>>();
        mockReactorB.SetupGet(p => p.Id).Returns(eventIdB);

        var mockReactorC = new Mock<IReceiveSubscription<int>>();
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
    public void Subscribe_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Subscribe(null);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushReactable<int>)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
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
        var mockReactorA = new Mock<IReceiveSubscription<int>>();
        var mockReactorB = new Mock<IReceiveSubscription<int>>();

        var expected = new[] { mockReactorA.Object, mockReactorB.Object };

        var sut = CreateSystemUnderTest();

        // Act
        var reactorUnsubscriberA = sut.Subscribe(mockReactorA.Object);
        var reactorUnsubscriberB = sut.Subscribe(mockReactorB.Object);

        var actual = sut.Subscriptions;

        // Assert
        actual.Should().BeEquivalentTo(expected);
        actual[0].Should().BeSameAs(mockReactorA.Object);
        actual[1].Should().BeSameAs(mockReactorB.Object);
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
            .WithMessage($"{nameof(PushReactable<int>)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingSomeEvents_UnsubscribesCorrectReactors()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = new Mock<ISubscription>();
        mockReactorA.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        var mockReactorB = new Mock<ISubscription>();
        mockReactorB.SetupGet(p => p.Id).Returns(eventNotToUnsubscribeFrom);

        var mockReactorC = new Mock<ISubscription>();
        mockReactorC.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockReactorA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockReactorB.Verify(m => m.OnUnsubscribe(), Times.Never);
        mockReactorC.Verify(m => m.OnUnsubscribe(), Times.Once);
        sut.Subscriptions.Should().HaveCount(1);
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingAllEventsOneAtATime_UnsubscribesCorrectReactors()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = new Mock<IReceiveSubscription<int>>();
        mockReactorA.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);
        mockReactorA.Setup(m => m.Unsubscribed).Returns(true);

        var mockReactorB = new Mock<IReceiveSubscription<int>>();
        mockReactorB.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);
        mockReactorB.Setup(m => m.Unsubscribed).Returns(true);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        sut.Unsubscribe(eventToUnsubscribeFrom);
        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockReactorA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockReactorB.Verify(m => m.OnUnsubscribe(), Times.Once);
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingInsideOnUnsubscribeReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initReactorA = new ReceiveSubscription<int>(
            id: mainId);

        var otherReactorA = new ReceiveSubscription<int>(id: otherId);
        var otherReactorB = new ReceiveSubscription<int>(id: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveSubscription<int>(
            id: mainId,
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
            .WithMessage($"{nameof(PushReactable<int>)} disposed.{Environment.NewLine}Object name: 'PushReactable'.");
    }

    [Fact]
    public void UnsubscribeAll_WhenInvoked_UnsubscribesFromAll()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockReactorA = new Mock<IReceiveSubscription<int>>();
        mockReactorA.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        var mockReactorB = new Mock<IReceiveSubscription<int>>();
        mockReactorB.SetupGet(p => p.Id).Returns(eventNotToUnsubscribeFrom);

        var mockReactorC = new Mock<IReceiveSubscription<int>>();
        mockReactorC.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorC.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorA.Object);

        sut.UnsubscribeAll();

        // Assert
        mockReactorA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockReactorB.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockReactorC.Verify(m => m.OnUnsubscribe(), Times.Once);
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void UnsubscribeAll_WhenUnsubscribingInsideOnUnsubscribeReactorAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initReactorA = new ReceiveSubscription<int>(
            id: mainId);

        var otherReactorA = new ReceiveSubscription<int>(id: otherId);
        var otherReactorB = new ReceiveSubscription<int>(id: otherId);

        var sut = CreateSystemUnderTest();

        var initReactorC = new ReceiveSubscription<int>(
            id: mainId,
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

        var mockReactorA = new Mock<IReceiveSubscription<int>>();
        mockReactorA.SetupGet(p => p.Id).Returns(eventIdA);

        var mockReactorB = new Mock<IReceiveSubscription<int>>();
        mockReactorB.SetupGet(p => p.Id).Returns(eventIdB);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
        mockReactorA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockReactorB.Verify(m => m.OnUnsubscribe(), Times.Once);

        sut.Subscriptions.Should().BeEmpty();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ReactableBaseFake CreateSystemUnderTest() => new ();
}
