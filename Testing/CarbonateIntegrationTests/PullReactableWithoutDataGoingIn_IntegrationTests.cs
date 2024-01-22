// <copyright file="PullReactableWithoutDataGoingIn_IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate.NonDirectional;
using Carbonate.OneWay;
using Carbonate.TwoWay;
using FluentAssertions;
using Xunit;

[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Integrations Tests Are Named In This Way")]
public class PullReactableWithoutDataGoingIn_IntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPushReactable_WithNoData_WorksCorrectly()
    {
        // Arrange
        var expectedNames = new[] { "test-name" };
        var unsubscribeInvoked = false;
        var onReceiveInvoked = false;
        var id = new Guid("306920ac-71be-4bbb-9846-36332d72412a");
        var sut = new PushReactable();

        var unsubscriber = sut.Subscribe(new ReceiveSubscription(
            id: id,
            name: "test-name",
            onReceive: () => onReceiveInvoked = true,
            onUnsubscribe: () => unsubscribeInvoked = true));

        // Act
        var actualNames = sut.Subscriptions.Select(s => s.Name).ToArray();
        sut.Push(id);
        sut.Unsubscribe(id);

        // Assert
        actualNames.Should().BeEquivalentTo(expectedNames);
        unsubscriber.Should().NotBeNull();
        onReceiveInvoked.Should().BeTrue();
        unsubscribeInvoked.Should().BeTrue();
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void WhenUsingPullReactable_WithData_WorksCorrectly()
    {
        // Arrange
        var expectedOutData = new SampleData { IntValue = 123, StringValue = "test-value" };
        var unsubscribeInvoked = false;
        var id = new Guid("222aedb3-51b2-414e-95b5-529b17691ccb");

        var sut = new PullReactable<SampleData>();

        var unsubscriber = sut.Subscribe(new RespondSubscription<SampleData>(
            id: id,
            name: "test-name",
            onRespond: () => new SampleData { IntValue = 123, StringValue = "test-value" },
            onUnsubscribe: () => unsubscribeInvoked = true));

        // Act
        var actualNames = sut.Subscriptions.Select(s => s.Name).ToArray();
        var actual = sut.Pull(id);
        sut.Unsubscribe(id);

        // Assert
        actualNames.Should().BeEquivalentTo(actualNames);
        unsubscriber.Should().NotBeNull();
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expectedOutData);
        unsubscribeInvoked.Should().BeTrue();
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void WhenUsingPushReactable_WithData_WorksCorrectly()
    {
        // Arrange
        var unsubscribeInvoked = false;
        var expectedInData = new SampleData { IntValue = 123, StringValue = "test-value", };
        var expectedNames = new[] { "test-name" };
        var testData = new SampleData { IntValue = 123, StringValue = "test-value", };
        SampleData? actualData = null;

        var id = new Guid("31ea1e20-5329-4bef-99e8-2dea2fb43484");
        var sut = new PushReactable<SampleData>();
        var unsubscriber = sut.Subscribe(
            new ReceiveSubscription<SampleData>(
                id: id,
                name: "test-name",
                onReceive: data => actualData = data,
                onUnsubscribe: () => unsubscribeInvoked = true));

        // Act
        var actualNames = sut.Subscriptions.Select(s => s.Name).ToArray();
        sut.Push(id, testData);
        sut.Unsubscribe(id);

        // Assert
        actualNames.Should().BeEquivalentTo(expectedNames);
        unsubscriber.Should().NotBeNull();
        actualData.Should().NotBeNull();
        actualData.Should().BeEquivalentTo(expectedInData);
        unsubscribeInvoked.Should().BeTrue();
        sut.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public void WhenUsingPushPullReactable_WithData_WorksCorrectly()
    {
        // Arrange
        var expectedNames = new[] { "test-name" };
        var actualInData = 0;
        const string expectedOutData = "test-value";
        var unsubscribeInvoked = false;
        var id = new Guid("d6a82cca-da34-48fa-bd1d-0a1e441cc647");
        var sut = new PushPullReactable<int, string>();

        var unsubscriber = sut.Subscribe(new ReceiveRespondSubscription<int, string>(
            id: id,
            name: "test-name",
            onReceiveRespond: data =>
            {
                actualInData = data;
                return "test-value";
            },
            onUnsubscribe: () => unsubscribeInvoked = true));

        // Act
        var actualNames = sut.Subscriptions.Select(s => s.Name).ToArray();
        var actualOutData = sut.PushPull(id, 123);
        sut.Unsubscribe(id);

        // Assert
        actualNames.Should().BeEquivalentTo(expectedNames);
        actualInData.Should().Be(123);
        actualOutData.Should().Be(expectedOutData);
        unsubscriber.Should().NotBeNull();
        unsubscribeInvoked.Should().BeTrue();
        sut.Subscriptions.Should().BeEmpty();
    }
    #endregion
}
