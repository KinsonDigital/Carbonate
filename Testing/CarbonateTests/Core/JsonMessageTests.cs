// <copyright file="JsonMessageTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Core;

using System.Text.Json;
using Carbonate.Core;
using Carbonate.Services;
using Helpers;
using FluentAssertions;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="JsonMessage{T}"/> class.
/// </summary>
public class JsonMessageTests
{
    private readonly Mock<ISerializerService> mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonMessageTests"/> class.
    /// </summary>
    public JsonMessageTests() => this.mockSerializerService = new Mock<ISerializerService>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullSerializerParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new JsonMessage<int>(null, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'serializerService')");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_WithNullJsonDataParam_ThrowsException(string jsonData)
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new JsonMessage<int>(this.mockSerializerService.Object, jsonData);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The string parameter must not be null or empty. (Parameter 'jsonData')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void GetData_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var testData = new PullTestData();
        this.mockSerializerService.Setup(m => m.Deserialize<PullTestData>("test-data"))
            .Returns(testData);

        var sut = new JsonMessage<PullTestData>(this.mockSerializerService.Object, "test-data");

        // Act
        var actual = sut.GetData();

        // Assert
        this.mockSerializerService.Verify(m => m.Deserialize<PullTestData>("test-data"), Times.Once);
        actual.Should().NotBeNull();
    }

    [Fact]
    public void GetData_WhenSerializationResultIsNull_InvokesOnErrorAction()
    {
        // Arrange
        PullTestData? nullData = null;

        var totalActionInvokes = 0;
        this.mockSerializerService.Setup(m => m.Deserialize<PullTestData>("test-data"))
            .Returns(nullData);

        var sut = new JsonMessage<PullTestData>(this.mockSerializerService.Object, "test-data");

        // Act
        _ = sut.GetData(e =>
        {
            e.Should().BeOfType<JsonException>();
            e.Message.Should().Be("Issues with the JSON deserialization process.");

            totalActionInvokes++;
        });

        // Assert
        totalActionInvokes.Should().Be(1);
    }
    #endregion
}
