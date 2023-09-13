// <copyright file="ReactableBaseTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using System.Diagnostics.CodeAnalysis;
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

        var mockSubA = new Mock<IReceiveSubscription<int>>();
        mockSubA.SetupGet(p => p.Id).Returns(eventIdA);

        var mockSubB = new Mock<IReceiveSubscription<int>>();
        mockSubB.SetupGet(p => p.Id).Returns(eventIdB);

        var mockSubC = new Mock<IReceiveSubscription<int>>();
        mockSubC.SetupGet(p => p.Id).Returns(eventIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);
        sut.Subscribe(mockSubC.Object);

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
    public void Subscribe_WithNullSub_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var act = () => sut.Subscribe(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'subscription')");
    }

    [Fact]
    public void Subscribe_WhenInvoked_SubscriptionsPropReturnsSubscriptions()
    {
        // Arrange
        var mockSubA = new Mock<IReceiveSubscription<int>>();
        var mockSubB = new Mock<IReceiveSubscription<int>>();

        var expected = new[] { mockSubA.Object, mockSubB.Object };

        var sut = CreateSystemUnderTest();

        // Act
        var unsubscriberA = sut.Subscribe(mockSubA.Object);
        var unsubscriberB = sut.Subscribe(mockSubB.Object);

        var actual = sut.Subscriptions;

        // Assert
        actual.Should().BeEquivalentTo(expected);
        actual[0].Should().BeSameAs(mockSubA.Object);
        actual[1].Should().BeSameAs(mockSubB.Object);
        unsubscriberA.Should().NotBeNull();
        unsubscriberB.Should().NotBeNull();
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
    public void Unsubscribe_WhenUnsubscribingSomeEvents_UnsubscribesCorrectSubscriptions()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();
        var eventNotToUnsubscribeFrom = Guid.NewGuid();

        var mockSubA = new Mock<ISubscription>();
        mockSubA.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        var mockSubB = new Mock<ISubscription>();
        mockSubB.SetupGet(p => p.Id).Returns(eventNotToUnsubscribeFrom);

        var mockSubC = new Mock<ISubscription>();
        mockSubC.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);
        sut.Subscribe(mockSubC.Object);

        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockSubA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockSubB.Verify(m => m.OnUnsubscribe(), Times.Never);
        mockSubC.Verify(m => m.OnUnsubscribe(), Times.Once);
        sut.Subscriptions.Should().HaveCount(1);
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingAllEventsOneAtATime_UnsubscribesCorrectSubscriptions()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();

        var mockSubA = new Mock<IReceiveSubscription<int>>();
        mockSubA.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);
        mockSubA.Setup(m => m.Unsubscribed).Returns(true);

        var mockSubB = new Mock<IReceiveSubscription<int>>();
        mockSubB.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);
        mockSubB.Setup(m => m.Unsubscribed).Returns(true);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);

        sut.Unsubscribe(eventToUnsubscribeFrom);
        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockSubA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockSubB.Verify(m => m.OnUnsubscribe(), Times.Once);
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingInsideOnUnsubscribeSubscriptionAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initSubA = new ReceiveSubscription<int>(id: mainId, _ => { });

        var otherSubA = new ReceiveSubscription<int>(id: otherId, _ => { });
        var otherSubB = new ReceiveSubscription<int>(id: otherId, _ => { });

        var sut = CreateSystemUnderTest();

        var initSubC = new ReceiveSubscription<int>(
            id: mainId,
            onReceive: _ => { },
            onUnsubscribe: () =>
            {
                sut.Unsubscribe(otherId);
            });

        sut.Subscribe(initSubA);
        sut.Subscribe(otherSubA);
        sut.Subscribe(otherSubB);
        sut.Subscribe(initSubC);

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

        var mockSubA = new Mock<IReceiveSubscription<int>>();
        mockSubA.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        var mockSubB = new Mock<IReceiveSubscription<int>>();
        mockSubB.SetupGet(p => p.Id).Returns(eventNotToUnsubscribeFrom);

        var mockSubC = new Mock<IReceiveSubscription<int>>();
        mockSubC.SetupGet(p => p.Id).Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubC.Object);
        sut.Subscribe(mockSubB.Object);
        sut.Subscribe(mockSubA.Object);

        sut.UnsubscribeAll();

        // Assert
        mockSubA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockSubB.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockSubC.Verify(m => m.OnUnsubscribe(), Times.Once);
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void UnsubscribeAll_WhenUnsubscribingInsideOnUnsubscribeSubscriptionAction_DoesNotThrowException()
    {
        // Arrange
        var mainId = new Guid("aaaaaaaa-a683-410a-b03e-8f8fe105b5af");
        var otherId = new Guid("bbbbbbbb-258d-4988-a169-4c23abf51c02");

        var initSubA = new ReceiveSubscription<int>(id: mainId, _ => { });

        var otherSubA = new ReceiveSubscription<int>(id: otherId, _ => { });
        var otherSubB = new ReceiveSubscription<int>(id: otherId, _ => { });

        var sut = CreateSystemUnderTest();

        var initSubC = new ReceiveSubscription<int>(
            id: mainId,
            onReceive: _ => { },
            onUnsubscribe: () =>
            {
                sut.Unsubscribe(otherId);
            });

        sut.Subscribe(initSubA);
        sut.Subscribe(otherSubA);
        sut.Subscribe(otherSubB);
        sut.Subscribe(initSubC);

        // Act
        var act = () => sut.UnsubscribeAll();

        // Assert
        act.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    [SuppressMessage("csharpsquid", "S3966", Justification = "Required for testing.")]
    public void Dispose_WhenInvoked_DisposesOfReactable()
    {
        // Arrange
        var eventIdA = Guid.NewGuid();
        var eventIdB = Guid.NewGuid();

        var mockSubA = new Mock<IReceiveSubscription<int>>();
        mockSubA.SetupGet(p => p.Id).Returns(eventIdA);

        var mockSubB = new Mock<IReceiveSubscription<int>>();
        mockSubB.SetupGet(p => p.Id).Returns(eventIdB);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
        mockSubA.Verify(m => m.OnUnsubscribe(), Times.Once);
        mockSubB.Verify(m => m.OnUnsubscribe(), Times.Once);

        sut.Subscriptions.Should().BeEmpty();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ReactableBaseFake CreateSystemUnderTest() => new ();
}
