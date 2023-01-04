// <copyright file="PullReactableTests.cs" company="KinsonDigital">
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
/// Tests the <see cref="PullReactable"/> class.
/// </summary>
public class PullReactableTests
{
    private readonly ISerializerService mockSerializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PullReactableTests"/> class.
    /// </summary>
    public PullReactableTests() => this.mockSerializerService = Substitute.For<ISerializerService>();

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
        var mockResult = Substitute.For<IResult>();
        mockResult.GetValue<ResultTestData>().Returns(new ResultTestData { Number = 123 });
        var respondId = Guid.NewGuid();

        var sut = CreateSystemUnderTest();
        sut.Subscribe(new RespondReactor(
            respondId: respondId,
            onRespond: () => mockResult));

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
        var mockResult = Substitute.For<IResult>();
        mockResult.GetValue<ResultTestData>().Returns(new ResultTestData { Number = 123 });

        var mockRespondReactor = Substitute.For<IRespondReactor>();
        mockRespondReactor.OnRespond(Arg.Any<JsonMessage>()).Returns(mockResult);

        var respondId = Guid.NewGuid();
        var testData = new PullTestData { Number = 123 };

        this.mockSerializerService.Serialize(Arg.Any<PullTestData>())
            .Returns(_ => JsonSerializer.Serialize(testData));

        var sut = CreateSystemUnderTest();
        sut.Subscribe(mockRespondReactor);

        // Act
        var actualResult = sut.Pull(testData, respondId);
        var actualData = actualResult?.GetValue<ResultTestData>();

        // Assert
        mockRespondReactor.Received(1).OnRespond(Arg.Any<JsonMessage>());
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
