// <copyright file="PushPullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.TwoWay;

using Carbonate.TwoWay;
using Carbonate.Core.TwoWay;
using FluentAssertions;
using Moq;
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

        var mockSubA = new Mock<IRespondSubscription<int, string>>();
        mockSubA.Name = nameof(mockSubA);
        mockSubA.SetupGet(p => p.Id).Returns(respondIdA);
        mockSubA.Setup(m => m.OnRespond(It.IsAny<int>()))
            .Returns(returnData);

        var mockSubB = new Mock<IRespondSubscription<int, string>>();
        mockSubB.Name = nameof(mockSubB);
        mockSubB.SetupGet(p => p.Id).Returns(respondIdB);
        mockSubB.Setup(m => m.OnRespond(It.IsAny<int>()))
            .Returns(returnData);

        const int data = 123;

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);

        // Act
        var actual = sut.PushPull(data, respondIdB);

        // Assert
        mockSubA.Verify(m => m.OnRespond(It.IsAny<int>()), Times.Never);
        mockSubB.Verify(m => m.OnRespond(It.IsAny<int>()), Times.Once);
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
