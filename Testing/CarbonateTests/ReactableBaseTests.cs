// <copyright file="ReactableBaseTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate.Core;
using Carbonate.Core.NonDirectional;
using Carbonate.Core.OneWay;
using Carbonate.OneWay;
using FluentAssertions;
using Helpers.Fakes;
using NSubstitute;
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
    public void SubscriptionIds_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var eventIdA = Guid.NewGuid();
        var eventIdB = Guid.NewGuid();

        var expected = new[] { eventIdA, eventIdB };

        var mockSubA = Substitute.For<IReceiveSubscription<int>>();
        mockSubA.Id.Returns(eventIdA);

        var mockSubB = Substitute.For<IReceiveSubscription<int>>();
        mockSubB.Id.Returns(eventIdB);

        var mockSubC = Substitute.For<IReceiveSubscription<int>>();
        mockSubC.Id.Returns(eventIdA);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);
        sut.Subscribe(mockSubC);

        // Act
        var actual = sut.SubscriptionIds;

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void SubscriptionNames_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var expected = new[] { "sub-1", "sub-2" };
        var sut = CreateSystemUnderTest();

        var mockSub1 = Substitute.For<IReceiveSubscription>();
        mockSub1.Name.Returns("sub-1");
        var mockSub2 = Substitute.For<IReceiveSubscription>();
        mockSub2.Name.Returns("sub-2");

        sut.Subscribe(mockSub1);
        sut.Subscribe(mockSub2);

        // Act
        var actual = sut.SubscriptionNames;

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
        var mockSubA = Substitute.For<IReceiveSubscription<int>>();
        var mockSubB = Substitute.For<IReceiveSubscription<int>>();

        var expected = new[] { mockSubA, mockSubB };

        var sut = CreateSystemUnderTest();

        // Act
        var unsubscriberA = sut.Subscribe(mockSubA);
        var unsubscriberB = sut.Subscribe(mockSubB);

        var actual = sut.Subscriptions;

        // Assert
        actual.Should().BeEquivalentTo(expected);
        actual[0].Should().BeSameAs(mockSubA);
        actual[1].Should().BeSameAs(mockSubB);
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

        var mockSubA = Substitute.For<ISubscription>();
        mockSubA.Id.Returns(eventToUnsubscribeFrom);

        var mockSubB = Substitute.For<ISubscription>();
        mockSubB.Id.Returns(eventNotToUnsubscribeFrom);

        var mockSubC = Substitute.For<ISubscription>();
        mockSubC.Id.Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);
        sut.Subscribe(mockSubC);

        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockSubA.Received(1).OnUnsubscribe();
        mockSubB.DidNotReceive().OnUnsubscribe();
        mockSubC.Received(1).OnUnsubscribe();
        sut.Subscriptions.Should().HaveCount(1);
    }

    [Fact]
    public void Unsubscribe_WhenUnsubscribingAllEventsOneAtATime_UnsubscribesCorrectSubscriptions()
    {
        // Arrange
        var eventToUnsubscribeFrom = Guid.NewGuid();

        var mockSubA = Substitute.For<IReceiveSubscription<int>>();
        mockSubA.Id.Returns(eventToUnsubscribeFrom);
        mockSubA.Unsubscribed.Returns(true);

        var mockSubB = Substitute.For<IReceiveSubscription<int>>();
        mockSubB.Id.Returns(eventToUnsubscribeFrom);
        mockSubB.Unsubscribed.Returns(true);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);

        sut.Unsubscribe(eventToUnsubscribeFrom);
        sut.Unsubscribe(eventToUnsubscribeFrom);

        // Assert
        mockSubA.Received(1).OnUnsubscribe();
        mockSubB.Received(1).OnUnsubscribe();
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

        var mockSubA = Substitute.For<IReceiveSubscription<int>>();
        mockSubA.Id.Returns(eventToUnsubscribeFrom);

        var mockSubB = Substitute.For<IReceiveSubscription<int>>();
        mockSubB.Id.Returns(eventNotToUnsubscribeFrom);

        var mockSubC = Substitute.For<IReceiveSubscription<int>>();
        mockSubC.Id.Returns(eventToUnsubscribeFrom);

        // Act
        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubC);
        sut.Subscribe(mockSubB);
        sut.Subscribe(mockSubA);

        sut.UnsubscribeAll();

        // Assert
        mockSubA.Received(1).OnUnsubscribe();
        mockSubB.Received(1).OnUnsubscribe();
        mockSubC.Received(1).OnUnsubscribe();
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

        var mockSubA = Substitute.For<IReceiveSubscription<int>>();
        mockSubA.Id.Returns(eventIdA);

        var mockSubB = Substitute.For<IReceiveSubscription<int>>();
        mockSubB.Id.Returns(eventIdB);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);

        // Act
        sut.Dispose();
        sut.Dispose();

        // Assert
        mockSubA.Received(1).OnUnsubscribe();
        mockSubB.Received(1).OnUnsubscribe();

        sut.Subscriptions.Should().BeEmpty();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushReactable{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ReactableBaseFake CreateSystemUnderTest() => new ();
}
