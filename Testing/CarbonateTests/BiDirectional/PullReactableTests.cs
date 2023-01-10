// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.BiDirectional;

using System.Text.Json;
using Carbonate.BiDirectional;
using Carbonate.Core;
using Carbonate.Core.BiDirectional;
using Carbonate.Services;
using FluentAssertions;
using Helpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="PullReactable{TDataIn,TDataOut}"/> class.
/// </summary>
public class PullReactableTests
{
    private readonly Mock<ISerializerService> mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PullReactableTests"/> class.
    /// </summary>
    public PullReactableTests() => this.mockSerializerService = new Mock<ISerializerService>();

    #region Method Tests
    [Fact]
    public void Pull_WithMatchingSubscription_ReturnsCorrectResult()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();

        const int msgData = 123;

        var mockResult = new Mock<IResult<ResultTestData>>();
        mockResult.Setup(m => m.GetValue(It.IsAny<Action<Exception>?>()))
            .Returns(new ResultTestData { Number = 123 });

        var mockReactorA = new Mock<IRespondReactor<int, ResultTestData>>();
        mockReactorA.Name = nameof(mockReactorA);
        mockReactorA.SetupGet(p => p.Id).Returns(respondIdA);
        mockReactorA.Setup(m => m.OnRespond(It.IsAny<IMessage<int>>()))
            .Returns(mockResult.Object);

        var mockReactorB = new Mock<IRespondReactor<int, ResultTestData>>();
        mockReactorB.Name = nameof(mockReactorB);
        mockReactorB.SetupGet(p => p.Id).Returns(respondIdB);
        mockReactorB.Setup(m => m.OnRespond(It.IsAny<IMessage<int>>()))
            .Returns(mockResult.Object);

        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns(JsonSerializer.Serialize(msgData));

        var mockMessage = new Mock<IMessage<int>>();
        mockMessage.Setup(m => m.GetData(It.IsAny<Action<Exception>?>()))
            .Returns<Action<Exception>?>(_ => msgData);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        var actualResult = sut.Pull(mockMessage.Object, respondIdB);
        var actualData = actualResult.GetValue();

        // Assert
        mockReactorA.Verify(m => m.OnRespond(It.IsAny<IMessage<int>>()), Times.Never);
        mockReactorB.Verify(m => m.OnRespond(It.IsAny<IMessage<int>>()), Times.Once);
        actualResult.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Number.Should().Be(123);
    }

    [Fact]
    public void Pull_WithNullReactorResponse_ReturnsEmptyResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var mockReactor = new Mock<IRespondReactor<int, ResultTestData>>();
        mockReactor.SetupGet(p => p.Id).Returns(respondId);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactor.Object);

        var mockMessage = new Mock<IMessage<int>>();

        // Act
        var actual = sut.Pull(mockMessage.Object, respondId);

        // Assert
        actual.Should().NotBeNull();
        actual.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Pul_WithNoMatchingSubscriptions_ReturnsEmptyResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var actual = sut.Pull(new Mock<IMessage<int>>().Object, Guid.NewGuid());

        // Assert
        actual.Should().NotBeNull();
        actual.IsEmpty.Should().BeTrue();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable{TDataIn,TDataOut}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PullReactable<int, ResultTestData> CreateSystemUnderTest() => new ();
}
