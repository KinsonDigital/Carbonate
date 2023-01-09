// <copyright file="PullReactableWithoutMsgIntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using Carbonate;
using Carbonate.UniDirectional;
using FluentAssertions;
using Xunit;

public class PullReactableWithoutMsgIntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPull_WithNoOutgoingMessage_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PullReactable<SampleData>();

        sut.Subscribe(new RespondReactor<SampleData>(
            respondId: respondId,
            name: "test-name",
            onRespond: () => ResultFactory.CreateResult(new SampleData { IntValue = 123, StringValue = "test-str" })));

        // Act
        var actualMsg = sut.Pull(respondId);
        var actualData = actualMsg.GetValue();

        // Assert
        actualMsg.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Should().BeOfType<SampleData>();
        actualData.IntValue.Should().Be(123);
        actualData.StringValue.Should().Be("test-str");
    }
    #endregion
}
