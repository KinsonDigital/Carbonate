﻿// <copyright file="PullReactableWithoutDataGoingIn_IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate.OneWay;
using FluentAssertions;
using Xunit;

[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Integrations Tests Are Named In This Way")]
public class PullReactableWithoutDataGoingIn_IntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPull_WithNoOutgoingData_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PullReactable<SampleData>();

        sut.Subscribe(new RespondSubscription<SampleData>(
            id: respondId,
            name: "test-name",
            onRespond: () => new SampleData { IntValue = 123, StringValue = "test-str" }));

        // Act
        var actual = sut.Pull(respondId);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeOfType<SampleData>();
        actual.IntValue.Should().Be(123);
        actual.StringValue.Should().Be("test-str");
    }
    #endregion
}
