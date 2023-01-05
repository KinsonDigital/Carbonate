// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateTests;

using System.Text.Json;
using Carbonate;
using Carbonate.Core;
using Carbonate.Services;
using FluentAssertions;
using Helpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="PullReactable"/> class.
/// </summary>
public class PullReactableTests
{
    private readonly Mock<ISerializerService> mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PullReactableTests"/> class.
    /// </summary>
    public PullReactableTests() => this.mockSerializerService = new Mock<ISerializerService>();

    #region Constructor
    [Fact]
    public void Ctor_WithNullSerializerServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new PullReactable(null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'serializerService')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Pull_WithNonGenericOverloadAndWhenResponseIsNull_ReturnsNull()
    {
        // Arrange
        var respondId = Guid.NewGuid();
        var mockReactor = new Mock<IRespondReactor>();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactor.Object);

        // Act
        var actual = sut.Pull(respondId);

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Pull_WithNonGenericOverloadAndWhenResponseIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var mockResult = new Mock<IResult>();
        mockResult.Setup(m => m.GetValue<ResultTestData>(It.IsAny<Action<Exception>?>()))
            .Returns(new ResultTestData { Number = 123 });

        var mockReactorA = new Mock<IRespondReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondId);
        mockReactorA.Setup(m => m.OnRespond()).Returns(mockResult.Object);

        var mockReactorB = new Mock<IRespondReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondId);
        mockReactorB.Setup(m => m.OnRespond()).Returns(mockResult.Object);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);

        // Act
        var actualResult = sut.Pull(respondId);
        var actualData = actualResult.GetValue<ResultTestData>();

        // Assert
        mockReactorA.Verify(m => m.OnRespond(), Times.Once);
        mockReactorB.Verify(m => m.OnRespond(), Times.Never);
        actualResult.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Number.Should().Be(123);
    }

    [Fact]
    public void Pull_WithNonGenericOverload_InvokesCorrectSubscriptions()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();
        var respondIdC = Guid.NewGuid();

        var mockReactorA = new Mock<IRespondReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondIdA);

        var mockReactorB = new Mock<IRespondReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondIdB);

        var mockReactorC = new Mock<IRespondReactor>();
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
    public void Pull_WithGenericOverloadAndWhenResponseIsNull_ReturnsNull()
    {
        // Arrange
        var respondId = Guid.NewGuid();
        var testData = new PullTestData { Number = 123 };

        var mockReactor = new Mock<IRespondReactor>();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactor.Object);

        // Act
        var actual = sut.Pull(testData, respondId);

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Pull_WithGenericOverloadAndWhenResponseIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();
        var testData = new PullTestData { Number = 123 };

        var mockResult = new Mock<IResult>();
        mockResult.Setup(m => m.GetValue<ResultTestData>(It.IsAny<Action<Exception>?>()))
            .Returns(new ResultTestData { Number = 123 });

        var mockReactorA = new Mock<IRespondReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondId);
        mockReactorA.Setup(m => m.OnRespond(It.IsAny<JsonMessage>()))
            .Returns(mockResult.Object);

        var mockReactorB = new Mock<IRespondReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondId);
        mockReactorB.Setup(m => m.OnRespond(It.IsAny<JsonMessage>()))
            .Returns(mockResult.Object);

        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns(JsonSerializer.Serialize(testData));

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);

        // Act
        var actualResult = sut.Pull(testData, respondId);
        var actualData = actualResult?.GetValue<ResultTestData>();

        // Assert
        mockReactorA.Verify(m => m.OnRespond(It.IsAny<JsonMessage>()), Times.Once);
        mockReactorB.Verify(m => m.OnRespond(It.IsAny<JsonMessage>()), Times.Never);
        actualResult.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Number.Should().Be(123);
    }

    [Fact]
    public void Pull_WithGenericOverload_InvokesCorrectSubscriptions()
    {
        // Arrange
        var respondIdA = Guid.NewGuid();
        var respondIdB = Guid.NewGuid();
        var respondIdC = Guid.NewGuid();
        var pullTestData = new PullTestData { Number = 123 };

        var mockReactorA = new Mock<IRespondReactor>();
        mockReactorA.SetupGet(p => p.Id).Returns(respondIdA);

        var mockReactorB = new Mock<IRespondReactor>();
        mockReactorB.SetupGet(p => p.Id).Returns(respondIdB);

        var mockReactorC = new Mock<IRespondReactor>();
        mockReactorC.SetupGet(p => p.Id).Returns(respondIdC);

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockReactorA.Object);
        sut.Subscribe(mockReactorB.Object);
        sut.Subscribe(mockReactorC.Object);

        // TODO: Might need to check result returned

        // Act
        sut.Pull(pullTestData, respondIdB);

        // Assert
        mockReactorA.Verify(m => m.OnRespond(It.IsAny<IMessage>()), Times.Never);
        mockReactorB.Verify(m => m.OnRespond(It.IsAny<IMessage>()), Times.Once);
        mockReactorC.Verify(m => m.OnRespond(It.IsAny<IMessage>()), Times.Never);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static PullReactable CreateSystemUnderTest() => new ();
}
