// <copyright file="PullReactableTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

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

        var sut = CreateSystemUnderTest();
        sut.Subscribe(new RespondReactor(
            respondId: respondId,
            onRespond: () => null));

        // Act
        var actual = sut.Pull(respondId);

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Pull_WithNonGenericOverloadAndWhenResponseIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var mockResult = new Mock<IResult>();
        mockResult.Setup(m => m.GetValue<ResultTestData>(It.IsAny<Action<Exception>?>()))
            .Returns(new ResultTestData { Number = 123 });
        var respondId = Guid.NewGuid();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(new RespondReactor(
            respondId: respondId,
            onRespond: () => mockResult.Object));

        // Act
        var actualResult = sut.Pull(respondId);
        var actualData = actualResult.GetValue<ResultTestData>();

        // Assert
        actualResult.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Number.Should().Be(123);
    }

    [Fact]
    public void Pull_WithGenericOverloadAndWhenResponseIsNull_ReturnsNull()
    {
        // Arrange
        var respondId = Guid.NewGuid();
        var testData = new PullTestData { Number = 123 };

        var sut = CreateSystemUnderTest();
        sut.Subscribe(new RespondReactor(
            respondId: respondId,
            onRespondMsg: _ => null));

        // Act
        var actual = sut.Pull(testData, respondId);

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Pull_WithGenericOverloadAndWhenResponseIsNotNull_ReturnsCorrectResult()
    {
        // Arrange
        var mockResult = new Mock<IResult>();
        mockResult.Setup(m => m.GetValue<ResultTestData>(It.IsAny<Action<Exception>?>()))
            .Returns(new ResultTestData { Number = 123 });

        var mockRespondReactor = new Mock<IRespondReactor>();
        mockRespondReactor.Setup(m => m.OnRespond(It.IsAny<JsonMessage>()))
            .Returns(mockResult.Object);

        var respondId = Guid.NewGuid();
        var testData = new PullTestData { Number = 123 };

        this.mockSerializerService.Setup(m => m.Serialize(It.IsAny<PullTestData>()))
            .Returns(JsonSerializer.Serialize(testData));

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockRespondReactor.Object);

        // Act
        var actualResult = sut.Pull(testData, respondId);
        var actualData = actualResult?.GetValue<ResultTestData>();

        // Assert
        mockRespondReactor.Verify(m => m.OnRespond(It.IsAny<JsonMessage>()), Times.Once);
        actualResult.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Number.Should().Be(123);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="PullReactable"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private PullReactable CreateSystemUnderTest()
        => new ();
}
