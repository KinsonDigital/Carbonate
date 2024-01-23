// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.OneWay;

using System.Diagnostics.CodeAnalysis;
using Carbonate.Core.OneWay;
using Carbonate.Exceptions;
using Carbonate.OneWay;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="PullReactable{TOut}"/> class.
/// </summary>
public class PullReactableTests
{
    #region Method Tests
    [Fact]
    public void Pull_WhenResponseIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        const string returnData = "return-value";

        var mockSubA = Substitute.For<IRespondSubscription<string>>();
        mockSubA.Id.Returns(respondId);
        mockSubA.OnRespond().Returns(returnData);

        var mockSubB = Substitute.For<IRespondSubscription<string>>();
        mockSubB.Id.Returns(respondId);
        mockSubB.OnRespond().Returns(returnData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);

        // Act
        var actual = sut.Pull(respondId);

        // Assert
        mockSubA.Received(1).OnRespond();
        mockSubB.DidNotReceive().OnRespond();
        actual.Should().NotBeNull();
        actual.Should().NotBeNull();
        actual.Should().Be("return-value");
    }

    [Fact]
    public void Pull_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.Pull(Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PullReactable<int>)} disposed.{Environment.NewLine}Object name: 'PullReactable'.");
    }

    [Fact]
    [SuppressMessage(
        "ReSharper",
        "AccessToModifiedClosure",
        Justification = "Required for testing.")]
    public void Pull_WhenUnsubscribingWhileProcessingNotifications_ThrowsException()
    {
        // Arrange
        var id = new Guid("296ad655-5ae3-4040-9602-9760af31362d");
        const string subName = "test-subscription";
        var expectedMsg = "The send notification process is currently in progress.";
        expectedMsg += $"\nThe subscription '{subName}' with id '{id}' could not be unsubscribed.";

        IDisposable? unsubscriber = null;
        var mockSubscription = Substitute.For<IRespondSubscription<string>>();
        mockSubscription.Id.Returns(id);
        mockSubscription.Name.Returns(subName);
        mockSubscription.When(x => x.OnRespond())
            .Do(_ =>
            {
                unsubscriber.Dispose();
            });

        var sut = CreateSystemUnderTest();
        unsubscriber = sut.Subscribe(mockSubscription);

        // Act
        var act = () => sut.Pull(id);

        // Assert
        act.Should().Throw<NotificationException>().WithMessage(expectedMsg);
    }

    [Fact]
    public void Pull_WhenSubscriptionExists_InvokesCorrectSubscriptions()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();
        var respondIdC = Guid.NewGuid();

        var mockSubA = Substitute.For<IRespondSubscription<string>>();
        mockSubA.Id.Returns(respondIdA);

        var mockSubB = Substitute.For<IRespondSubscription<string>>();
        mockSubB.Id.Returns(respondIdB);

        var mockSubC = Substitute.For<IRespondSubscription<string>>();
        mockSubC.Id.Returns(respondIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);
        sut.Subscribe(mockSubC);

        // Act
        sut.Pull(respondIdB);

        // Assert
        mockSubA.DidNotReceive().OnRespond();
        mockSubB.Received(1).OnRespond();
        mockSubC.DidNotReceive().OnRespond();
    }

    [Fact]
    public void Pull_WhenSubscriptionDoesNotExist_ReturnsCorrectDefaultResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var actual = sut.Pull(Guid.NewGuid());

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Pull_WhenPullingDataThatThrowsException_InvokesOnErrorSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var sut = CreateSystemUnderTest();

        // Act & Assert
        sut.Subscribe(new RespondSubscription<string>(
                id: id,
                name: "test-name",
                onRespond: () => throw new Exception("test-exception"),
                onError: e =>
                {
                    e.Should().NotBeNull();
                    e.Message.Should().Be("test-exception");
                }));

        sut.Pull(id);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable{TOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PullReactable<string> CreateSystemUnderTest() => new ();
}
