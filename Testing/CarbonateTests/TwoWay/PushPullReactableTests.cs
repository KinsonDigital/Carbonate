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
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();

        const string returnData = "return-value";

        var mockSubA = Substitute.For<IReceiveRespondSubscription<int, string>>();
        mockSubA.Id.Returns(respondIdA);
        mockSubA.OnRespond(Arg.Any<int>()).Returns(returnData);

        var mockSubB = Substitute.For<IReceiveRespondSubscription<int, string>>();
        mockSubB.Id.Returns(respondIdB);
        mockSubB.OnRespond(Arg.Any<int>()).Returns(returnData);

        const int data = 123;

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA);
        sut.Subscribe(mockSubB);

        // Act
        var actual = sut.PushPull(data, respondIdB);

        // Assert
        mockSubA.DidNotReceive().OnRespond(Arg.Any<int>());
        mockSubB.Received().OnRespond(Arg.Any<int>());
        actual.Should().NotBeNull();
        actual.Should().NotBeNull();
        actual.Should().Be("return-value");
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
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PushPullReactable{TIn,TOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PushPullReactable<int, string> CreateSystemUnderTest() => new ();
}
