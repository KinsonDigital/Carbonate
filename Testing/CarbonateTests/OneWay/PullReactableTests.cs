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

        var mockReactorA = new Mock<IRespondReactor<string>>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondId);
        mockReactorA.Setup(m => m.OnRespond()).Returns(returnData);

        var mockReactorB = new Mock<IRespondReactor<string>>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondId);
        mockReactorB.Setup(m => m.OnRespond()).Returns(returnData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        var actual = sut.Pull(respondId);

        // Assert
        mockReactorA.Verify(m => m.OnRespond(), Times.Once);
        mockReactorB.Verify(m => m.OnRespond(), Times.Never);
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

        var mockReactorA = new Mock<IRespondReactor<string>>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondIdA);

        var mockReactorB = new Mock<IRespondReactor<string>>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondIdB);

        var mockReactorC = new Mock<IRespondReactor<string>>();
        mockReactorC.SetupGet(p => p.Id).Returns(respondIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // Act
        sut.Pull(respondIdB);

        // Assert
        mockReactorA.Verify(m => m.OnRespond(), Times.Never);
        mockReactorB.Verify(m => m.OnRespond(), Times.Once);
        mockReactorC.Verify(m => m.OnRespond(), Times.Never);
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
