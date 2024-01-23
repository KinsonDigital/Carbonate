// <copyright file="PushPullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.TwoWay;

using System.Diagnostics.CodeAnalysis;
using Carbonate.TwoWay;
using Carbonate.Core.TwoWay;
using Carbonate.Exceptions;
using FluentAssertions;
using NSubstitute;
using Xunit;

/// <summary>
/// Tests the <see cref="PushPullReactable{TIn,TOut}"/> class.
/// </summary>
public class PushPullReactableTests
{
    #region Method Tests
    [Fact]
    public void Pull_WithMatchingSubscription_ReturnsCorrectResult()
    {
        // Arrange
        var respondIdA = new Guid("a1c34c09-2a5c-4482-8699-4586ee31a126");
        var respondIdB = new Guid("e8804d3e-a002-4ed8-8263-038668190141");

        const string returnData = "return-value";

        var mockSubA = Substitute.For<IReceiveRespondSubscription<int, string>>();
        mockSubA.Id.Returns(respondIdA);
        mockSubA.OnRespond(Arg.Any<int>()).Returns(returnData);

        var mockSubB = Substitute.For<IReceiveRespondSubscription<int, string>>();
        mockSubB.Id.Returns(respondIdB);
        mockSubB.OnRespond(Arg.Any<int>()).Returns(returnData);

        const int data = 654;

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);

        // Act
        var actual = sut.PushPull(respondIdB, data);

        // Assert
        mockSubA.DidNotReceive().OnRespond(321);
        mockSubB.Received().OnRespond(654);
        actual.Should().NotBeNull();
        actual.Should().NotBeNull();
        actual.Should().Be("return-value");
    }

    [Fact]
    public void PushPull_WhenInvokedAfterDisposal_ThrowsException()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Dispose();

        // Act
        var act = () => sut.PushPull(Guid.Empty, 123);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushPullReactable<int, string>)} disposed.{Environment.NewLine}Object name: 'PushPullReactable'.");
    }

    [Fact]
    [SuppressMessage(
        "ReSharper",
        "AccessToModifiedClosure",
        Justification = "Required for testing.")]
    public void Push_WhenUnsubscribingWhileProcessingNotifications_ThrowsException()
    {
        // Arrange
        var id = new Guid("37cfea9d-3ca6-4a67-8a65-ea138cd04aef");
        const string subName = "test-subscription";
        var expectedMsg = "The send notification process is currently in progress.";
        expectedMsg += $"\nThe subscription '{subName}' with id '{id}' could not be unsubscribed.";

        IDisposable? unsubscriber = null;
        var mockSubscription = Substitute.For<IReceiveRespondSubscription<int, string>>();
        mockSubscription.Id.Returns(id);
        mockSubscription.Name.Returns(subName);
        mockSubscription.When(x => x.OnRespond(123))
            .Do(_ =>
            {
                unsubscriber.Dispose();
            });

        var sut = CreateSystemUnderTest();
        unsubscriber = sut.Subscribe(mockSubscription);

        // Act
        var act = () => sut.PushPull(id, 123);

        // Assert
        act.Should().Throw<NotificationException>().WithMessage(expectedMsg);
    }

    [Fact]
    public void Pull_WithNoMatchingSubscription_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new PushPullReactable<int, int>();

        // Act
        var actual = sut.PushPull(Guid.NewGuid(), 123);

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    public void Pull_WhenPullingDataThatThrowsException_InvokesOnErrorSubscription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var sut = CreateSystemUnderTest();

        // Act & Assert
        sut.Subscribe(new ReceiveRespondSubscription<int, string>(
            id: id,
            name: "test-name",
            onReceiveRespond: _ => throw new Exception("test-exception"),
            onError: e =>
            {
                e.Should().NotBeNull();
                e.Message.Should().Be("test-exception");
            }));

        sut.PushPull(id, 123);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushPullReactable{TIn,TOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushPullReactable<int, string> CreateSystemUnderTest() => new ();
}
