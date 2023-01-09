// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.UniDirectional;

using Carbonate.Core;
using Carbonate.Core.UniDirectional;
using Carbonate.UniDirectional;
using FluentAssertions;
using Helpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="PullReactable{TDataOut}"/> class.
/// </summary>
public class PullReactableTests
{
    #region Method Tests
    [Fact]
    public void Pull_WhenResponseIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var mockResult = new Mock<IResult<ResultTestData>>();
        mockResult.Setup(m => m.GetValue(It.IsAny<Action<Exception>?>()))
            .Returns(new ResultTestData { Number = 123 });

        var mockReactorA = new Mock<IRespondReactor<ResultTestData>>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondId);
        mockReactorA.Setup(m => m.OnRespond()).Returns(mockResult.Object);

        var mockReactorB = new Mock<IRespondReactor<ResultTestData>>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondId);
        mockReactorB.Setup(m => m.OnRespond()).Returns(mockResult.Object);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        var actualResult = sut.Pull(respondId);
        var actualData = actualResult.GetValue();

        // Assert
        mockReactorA.Verify(m => m.OnRespond(), Times.Once);
        mockReactorB.Verify(m => m.OnRespond(), Times.Never);
        actualResult.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Number.Should().Be(123);
    }

    [Fact]
    public void Pull_WhenInvoked_InvokesCorrectSubscriptions()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();
        var respondIdC = Guid.NewGuid();

        var mockReactorA = new Mock<IRespondReactor<ResultTestData>>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondIdA);

        var mockReactorB = new Mock<IRespondReactor<ResultTestData>>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondIdB);

        var mockReactorC = new Mock<IRespondReactor<ResultTestData>>();
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
    public void Pull_WithNoSubscriptions_ReturnsEmptyResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var actual = sut.Pull(It.IsAny<Guid>());

        // Assert
        actual.Should().NotBeNull();
        actual.IsEmpty.Should().BeTrue();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable{TDataOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PullReactable<ResultTestData> CreateSystemUnderTest() => new ();
}
