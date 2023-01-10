// <copyright file="PullReactableWithMsgIntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using Carbonate;
using Carbonate.BiDirectional;
using FluentAssertions;
using Xunit;

public class PullReactableWithMsgIntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPull_WithOutgoingMessage_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PullReactable<int, SampleData>();

        sut.Subscribe(new RespondReactor<int, SampleData>(
            respondId: respondId,
            name: "test-name",
            onRespondMsg: _ => ResultFactory.CreateResult(new SampleData { IntValue = 123, StringValue = "test-str" })));

        var msg = MessageFactory.CreateMessage(123);

        // Act
        var actualMsg = sut.Pull(msg, respondId);
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
