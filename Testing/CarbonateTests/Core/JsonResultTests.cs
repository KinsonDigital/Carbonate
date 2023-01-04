// <copyright file="JsonResultTests.cs" company="KinsonDigital">
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
/// Tests the <see cref="JsonResult"/>.
/// </summary>
public class JsonResultTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullSerializerServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new JsonResult(null, null);
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
            _ = new JsonResult(new Mock<ISerializerService>().Object, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The string parameter must not be null or empty. (Parameter 'jsonData')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void GetValue_WithNullSerializationResultAndWithNonNullOnErrorParam_InvokesOnErrorActionParameter()
    {
        // Arrange
        ResultTestData? resultData = null;

        var mockSerializerService = new Mock<ISerializerService>();

        var sut = new JsonResult(mockSerializerService.Object, "test-value");

        // Act
        var act = () => resultData = sut.GetValue<ResultTestData>(e => throw e);

        // Assert
        act.Should().Throw<JsonException>()
            .WithMessage("Issues with the JSON deserialization process.");
        resultData.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNullSerializationResultAndWithNullOnErrorParam_ReturnsNull()
    {
        // Arrange
        var mockSerializerService = new Mock<ISerializerService>();

        var sut = new JsonResult(mockSerializerService.Object, "test-value");

        // Act
        var resultData = sut.GetValue<ResultTestData>();

        // Assert
        resultData.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNonNullSerializationResult_ReturnsCorrectResult()
    {
        // Arrange
        var mockSerializerService = new Mock<ISerializerService>();
        mockSerializerService.Setup(m => m.Deserialize<ResultTestData>(It.IsAny<string>()))
            .Returns(new ResultTestData { Number = 123 });

        var sut = new JsonResult(mockSerializerService.Object, "test-value");

        // Act
        var resultData = sut.GetValue<ResultTestData>();

        // Assert
        mockSerializerService.Verify(m => m.Deserialize<ResultTestData>("test-value"), Times.Once);
        resultData.Should().NotBeNull();
        resultData.Number.Should().Be(123);
    }
    #endregion
}
