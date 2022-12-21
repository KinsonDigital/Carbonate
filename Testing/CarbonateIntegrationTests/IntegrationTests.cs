// <copyright file="IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable AccessToModifiedClosure
namespace CarbonateIntegrationTests;

using Carbonate;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests all of the components integrated together.
/// </summary>
public class IntegrationTests
{
    [Fact]
    public void WithSingleEventID_And_WithSingleSubscription_And_WithSingleUnsubscribe_ReturnsCorrectResults()
    {
        // Arrange
        SampleData? expectedData = null;
        var eventId = new Guid("98a879d4-e819-41da-80e4-a1b459b3e43f");

        IDisposable? unsubscriber = null;

        var reactable = new Reactable();

        unsubscriber = reactable.Subscribe(new Reactor(
            eventId,
            onNext: data =>
            {
                expectedData = data.GetData<SampleData>();
            }, onCompleted: () => unsubscriber?.Dispose()));

        var sampleData = new SampleData { StringValue = "test-string", IntValue = 123 };

        // Act
        reactable.Push(sampleData, eventId);
        reactable.Unsubscribe(eventId);

        // Assert
        reactable.Reactors.Should().HaveCount(0);
        expectedData.Should().NotBeNull();
        expectedData.StringValue.Should().Be("test-string");
        expectedData.IntValue.Should().Be(123);
    }

    [Fact]
    public void WithSingleEventID_And_WithMultipleSubscriptions_And_WithSingleEventUnsubscribe_UnsubscribesFromAll()
    {
        // Arrange
        var expectedData = new List<SampleData?>();

        var eventId = new Guid("fc71094c-5b93-4af8-aec9-5932430c041b");

        var reactable = new Reactable();

        // Subscription
        for (var i = 0; i < 10; i++)
        {
            IDisposable? unsubscriber = null;
            unsubscriber = reactable.Subscribe(new Reactor(
                    eventId,
                    onNext: data =>
                    {
                        expectedData.Add(data.GetData<SampleData>());
                    },
                    onCompleted: () =>
                    {
                        unsubscriber?.Dispose();
                    }));
        }

        var sampleData = new SampleData { StringValue = "test-string", IntValue = 123 };

        // Act
        reactable.Push(sampleData, eventId);

        reactable.Unsubscribe(eventId);

        // Assert
        reactable.Reactors.Should().HaveCount(0);
        expectedData.Should().HaveCount(10);

        expectedData.Should().AllSatisfy(item =>
        {
            item.Should().BeEquivalentTo(sampleData);
        }, "the same data was pushed to all subscribers.");
    }

    [Fact]
    public void WithMultipleEventIDs_And_WithMultipleSubscriptions_And_WithSingleEventUnsubscribe_ReturnsCorrectResults()
    {
        // Arrange
        SampleData? expectedDataA = null;
        SampleData? expectedDataB = null;

        var eventIdA = new Guid("98a879d4-e819-41da-80e4-a1b459b3e43f");
        var eventIdB = new Guid("24f3dbdd-c832-4ba1-83f7-b1a922fdd448");

        IDisposable? unsubscriberA = null;
        IDisposable? unsubscriberB = null;

        var reactable = new Reactable();

        // Subscription A
        unsubscriberA = reactable.Subscribe(new Reactor(
            eventIdA,
            onNext: data =>
            {
                expectedDataA = data.GetData<SampleData>();
            }, onCompleted: () => unsubscriberA?.Dispose()));

        // Subscription B
        unsubscriberB = reactable.Subscribe(new Reactor(
            eventIdB,
            onNext: data =>
            {
                expectedDataB = data.GetData<SampleData>();
            }, onCompleted: () => unsubscriberB?.Dispose()));

        var sampleDataA = new SampleData { StringValue = "test-string-A", IntValue = 123 };
        var sampleDataB = new SampleData { StringValue = "test-string-B", IntValue = 456 };

        // Act
        reactable.Push(sampleDataA, eventIdA);
        reactable.Push(sampleDataB, eventIdB);

        reactable.Unsubscribe(eventIdA);

        // Assert
        reactable.Reactors.Should().HaveCount(1);

        expectedDataA.Should().NotBeNull();
        expectedDataA.StringValue.Should().Be("test-string-A");
        expectedDataA.IntValue.Should().Be(123);

        expectedDataB.Should().NotBeNull();
        expectedDataB.StringValue.Should().Be("test-string-B");
        expectedDataB.IntValue.Should().Be(456);
    }
}
