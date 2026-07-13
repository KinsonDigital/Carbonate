// <copyright file="PushPullReactableWithData_IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate.TwoWay;
using Shouldly;
using Xunit;

[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Integrations Tests Are Named In This Way")]
public class PushPullReactableWithData_IntegrationTests
{
    #region Method Tests
    [Fact]
    public void WhenUsingPushPull_WithOutgoingData_ReturnsCorrectResult()
    {
        // Arrange
        var respondId = Guid.NewGuid();

        var sut = new PushPullReactable<int, SampleData>();

        sut.Subscribe(new ReceiveRespondSubscription<int, SampleData>(
            id: respondId,
            name: "test-name",
            onReceiveRespond: _ => new SampleData { IntValue = 123, StringValue = "test-str" }));

        // Act
        var actual = sut.PushPull(respondId, 123);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBeOfType<SampleData>();
        actual.IntValue.ShouldBe(123);
        actual.StringValue.ShouldBe("test-str");
    }
    #endregion
}
