// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.OneWay;

using Carbonate.Core.OneWay;
using Carbonate.OneWay;
using FluentAssertions;
using Moq;
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

        var mockSubA = new Mock<IRespondSubscription<string>>();
        mockSubA.SetupGet(p => p.Id).Returns(respondId);
        mockSubA.Setup(m => m.OnRespond()).Returns(returnData);

        var mockSubB = new Mock<IRespondSubscription<string>>();
        mockSubB.SetupGet(p => p.Id).Returns(respondId);
        mockSubB.Setup(m => m.OnRespond()).Returns(returnData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);

        // Act
        var actual = sut.Pull(respondId);

        // Assert
        mockSubA.Verify(m => m.OnRespond(), Times.Once);
        mockSubB.Verify(m => m.OnRespond(), Times.Never);
        actual.Should().NotBeNull();
        actual.Should().NotBeNull();
        actual.Should().Be("return-value");
    }

    [Fact]
    public void Pull_WhenSubscriptionExists_InvokesCorrectSubscriptions()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();
        var respondIdC = Guid.NewGuid();

        var mockSubA = new Mock<IRespondSubscription<string>>();
        mockSubA.SetupGet(p => p.Id).Returns(respondIdA);

        var mockSubB = new Mock<IRespondSubscription<string>>();
        mockSubB.SetupGet(p => p.Id).Returns(respondIdB);

        var mockSubC = new Mock<IRespondSubscription<string>>();
        mockSubC.SetupGet(p => p.Id).Returns(respondIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockSubA.Object);
        sut.Subscribe(mockSubB.Object);
        sut.Subscribe(mockSubC.Object);

        // Act
        sut.Pull(respondIdB);

        // Assert
        mockSubA.Verify(m => m.OnRespond(), Times.Never);
        mockSubB.Verify(m => m.OnRespond(), Times.Once);
        mockSubC.Verify(m => m.OnRespond(), Times.Never);
    }

    [Fact]
    public void Pull_WhenSubscriptionDoesNotExist_ReturnsCorrectDefaultResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var actual = sut.Pull(It.IsAny<Guid>());

        // Assert
        actual.Should().BeNull();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable{TOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PullReactable<string> CreateSystemUnderTest() => new ();
}
