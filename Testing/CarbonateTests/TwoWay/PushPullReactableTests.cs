// <copyright file="PushPullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.TwoWay;

using Carbonate.TwoWay;
using Carbonate.Core.TwoWay;
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
        var actual = sut.PushPull(data, respondIdB);

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
        var act = () => sut.PushPull(123, Guid.Empty);

        // Assert
        act.Should().Throw<ObjectDisposedException>()
            .WithMessage($"{nameof(PushPullReactable<int, string>)} disposed.{Environment.NewLine}Object name: 'PushPullReactable'.");
    }

    [Fact]
    public void Pull_WithNoMatchingSubscription_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new PushPullReactable<int, int>();

        // Act
        var actual = sut.PushPull(123, Guid.NewGuid());

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

        sut.PushPull(123, id);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushPullReactable{TIn,TOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushPullReactable<int, string> CreateSystemUnderTest() => new ();
}
