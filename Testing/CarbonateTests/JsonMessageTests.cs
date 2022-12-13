// <copyright file="JsonMessageTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using Carbonate;
using Carbonate.Services;
using CarbonateTests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CarbonateTests;

/// <summary>
/// Tests the <see cref="JsonMessage"/> class.
/// </summary>
public class JsonMessageTests
{
    private readonly ISerializer mockSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonMessageTests"/> class.
    /// </summary>
    public JsonMessageTests() => this.mockSerializer = Substitute.For<ISerializer>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullSerializerParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new JsonMessage(null, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'serializer')");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_WithNullJsonDataParam_ThrowsException(string jsonData)
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new JsonMessage(this.mockSerializer, jsonData);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The string parameter must not be null or empty. (Parameter 'jsonData')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Deserialize_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var testData = default(TestData);
        this.mockSerializer.Deserialize<TestData>(Arg.Any<string>()).Returns(testData);

        var sut = new JsonMessage(this.mockSerializer, "test-data");

        // Act
        sut.Deserialize<TestData>();

        // Assert
        this.mockSerializer.Received(1).Deserialize<TestData>("test-data");
    }
    #endregion
}
