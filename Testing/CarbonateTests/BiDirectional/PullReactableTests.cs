// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.BiDirectional;

using Carbonate.BiDirectional;
using Carbonate.Core.BiDirectional;
using FluentAssertions;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="PullReactable{TDataIn,TDataOut}"/> class.
/// </summary>
public class PullReactableTests
{
    #region Method Tests
    [Fact]
    public void Pull_WithMatchingSubscription_ReturnsCorrectResult()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();

        const string returnData = "return-value";

        var mockReactorA = new Mock<IRespondReactor<int, string>>();
        mockReactorA.Name = nameof(mockReactorA);
        mockReactorA.SetupGet(p => p.Id).Returns(respondIdA);
        mockReactorA.Setup(m => m.OnRespond(It.IsAny<int>()))
            .Returns(returnData);

        var mockReactorB = new Mock<IRespondReactor<int, string>>();
        mockReactorB.Name = nameof(mockReactorB);
        mockReactorB.SetupGet(p => p.Id).Returns(respondIdB);
        mockReactorB.Setup(m => m.OnRespond(It.IsAny<int>()))
            .Returns(returnData);

        const int data = 123;

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        var actual = sut.Pull(data, respondIdB);

        // Assert
        mockReactorA.Verify(m => m.OnRespond(It.IsAny<int>()), Times.Never);
        mockReactorB.Verify(m => m.OnRespond(It.IsAny<int>()), Times.Once);
        actual.Should().NotBeNull();
        actual.Should().NotBeNull();
        actual.Should().Be("return-value");
    }

    [Fact]
    public void Pull_WithNoMatchingSubscription_ReturnsCorrectResult()
    {
        // Arrange
        var sut = new PullReactable<int, int>();

        // Act
        var actual = sut.Pull(123, Guid.NewGuid());

        // Assert
        actual.Should().Be(0);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable{TDataIn,TDataOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PullReactable<int, string> CreateSystemUnderTest() => new ();
}
