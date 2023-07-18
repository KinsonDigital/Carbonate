// <copyright file="PullReactableWithData_IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate.BiDirectional;
using FluentAssertions;
using Xunit;

[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Integrations Tests Are Named In This Way")]
public class PullReactableWithData_IntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPull_WithOutgoingData_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PullReactable<int, SampleData>();

        sut.Subscribe(new RespondReactor<int, SampleData>(
            respondId: respondId,
            name: "test-name",
            onRespondData: _ => new SampleData { IntValue = 123, StringValue = "test-str" }));

        // Act
        var actual = sut.Pull(123, respondId);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<SampleData>();
        actual.IntValue.Should().Be(123);
        actual.StringValue.Should().Be("test-str");
    }
    #endregion
}
