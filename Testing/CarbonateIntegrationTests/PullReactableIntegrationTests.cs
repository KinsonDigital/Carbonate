// <copyright file="PullReactableIntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using Carbonate;
using FluentAssertions;
using Xunit;

public class PullReactableIntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPull_WithNoOutgoingMessage_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PullReactable();

        sut.Subscribe(new RespondReactor(
            respondId: respondId,
            name: "test-name",
            onRespond: () => ResultFactory.CreateResult(new SampleData { IntValue = 123, StringValue = "test-str" })));

        // Act
        var actualMsg = sut.Pull(respondId);
        var actualData = actualMsg.GetValue<SampleData>();

        // Assert
        actualMsg.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Should().BeOfType<SampleData>();
        actualData.IntValue.Should().Be(123);
        actualData.StringValue.Should().Be("test-str");
    }

    [Fact]
    public void WhenUsingPull_WithOutgoingMessage_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PullReactable();

        sut.Subscribe(new RespondReactor(
            respondId: respondId,
            name: "test-name",
            onRespondMsg: msg =>
            {
                var data = msg.GetData<SampleData>();

                data.Should().NotBeNull();

                return ResultFactory.CreateResult(new SampleData { IntValue = 123, StringValue = "test-str" });
            }));

        var pullData = new PullData { Number = 123 };

        // Act
        var actualMsg = sut.Pull(pullData, respondId);
        var actualData = actualMsg.GetValue<SampleData>();

        // Assert
        actualMsg.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Should().BeOfType<SampleData>();
        actualData.IntValue.Should().Be(123);
        actualData.StringValue.Should().Be("test-str");
    }
    #endregion
}
