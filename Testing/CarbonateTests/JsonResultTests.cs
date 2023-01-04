// <copyright file="JsonResultTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using System.Text.Json;
using Carbonate;
using Carbonate.Services;
using FluentAssertions;
using Helpers;
using NSubstitute;
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
            _ = new JsonResult(Substitute.For<ISerializerService>(), null);
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

        var mockSerializerService = Substitute.For<ISerializerService>();

        var sut = new JsonResult(mockSerializerService, "test-value");

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
        var mockSerializerService = Substitute.For<ISerializerService>();

        var sut = new JsonResult(mockSerializerService, "test-value");

        // Act
        var resultData = sut.GetValue<ResultTestData>();

        // Assert
        resultData.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithNonNullSerializationResult_ReturnsCorrectResult()
    {
        // Arrange
        var mockSerializerService = Substitute.For<ISerializerService>();
        mockSerializerService.Deserialize<ResultTestData>(Arg.Any<string>())
            .Returns(new ResultTestData { Number = 123 });

        var sut = new JsonResult(mockSerializerService, "test-value");

        // Act
        var resultData = sut.GetValue<ResultTestData>();

        // Assert
        mockSerializerService.Received(1).Deserialize<ResultTestData>("test-value");
        resultData.Should().NotBeNull();
        resultData.Number.Should().Be(123);
    }
    #endregion
}
