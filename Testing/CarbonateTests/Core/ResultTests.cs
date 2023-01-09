// <copyright file="ResultTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Core;

using System.Text.Json;
using Carbonate.Core;
using Carbonate.Services;
using FluentAssertions;
using Helpers;
using Moq;
using Xunit;

/// <summary>
/// Tests the <see cref="Result{T}"/>.
/// </summary>
public class ResultTests
{
    private Mock<ISerializerService> mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultTests"/> class.
    /// </summary>
    public ResultTests() => this.mockSerializerService = new Mock<ISerializerService>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullSerializerServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Result<ResultTestData>(null, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'serializerService')");
    }

    [Fact]
    public void Ctor_WithNullJSONDataParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Result<ResultTestData>(new Mock<ISerializerService>().Object, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The string parameter must not be null or empty. (Parameter 'jsonData')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void GetValue_WhenEmpty_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.IsEmpty = true;

        // Act
        var actual = sut.GetValue();

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNullSerializationResultAndWithNonNullOnErrorParam_InvokesOnErrorActionParameter()
    {
        // Arrange
        ResultTestData? resultData = null;

        var sut = CreateSystemUnderTest();

        // Act
        var act = () => resultData = sut.GetValue(e => throw e);

        // Assert
        act.Should().Throw<JsonException>()
            .WithMessage("Issues with the JSON deserialization process.");
        resultData.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNullSerializationResultAndWithNullOnErrorParam_ReturnsNull()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var resultData = sut.GetValue();

        // Assert
        resultData.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNonNullSerializationResult_ReturnsCorrectResult()
    {
        // Arrange
        this.mockSerializerService.Setup(m => m.Deserialize<ResultTestData>(It.IsAny<string>()))
            .Returns(new ResultTestData { Number = 123 });

        var sut = CreateSystemUnderTest("test-value");

        // Act
        var resultData = sut.GetValue();

        // Assert
        this.mockSerializerService.Verify(m => m.Deserialize<ResultTestData>("test-value"), Times.Once);
        resultData.Should().NotBeNull();
        resultData.Number.Should().Be(123);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="Result{T}"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private Result<ResultTestData> CreateSystemUnderTest(string jsonData = "default-value")
        => new (this.mockSerializerService.Object, jsonData);
}
