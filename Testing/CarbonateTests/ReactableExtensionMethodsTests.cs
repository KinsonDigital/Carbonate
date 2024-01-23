// <copyright file="ReactableExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using System.Diagnostics.CodeAnalysis;
using Carbonate;
using Carbonate.NonDirectional;
using Carbonate.OneWay;
using Carbonate.TwoWay;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests the <see cref="ReactableExtensionMethods"/> class.
/// </summary>
public class ReactableExtensionMethodsTests
{
    #region Test Data
#pragma warning disable SA1514
    /// <summary>
    /// Gets unsubscribe and on error delegate data for testing.
    /// </summary>
    public static TheoryData<Action?, Action<Exception>?> SubscribeErrorData =>
        new ()
        {
            { null, null },
            { null, _ => { } },
            { () => { }, null },
            { () => { }, _ => { } },
        };
#pragma warning restore SA1514
    #endregion

    #region Method Exception Tests
    #region CreateNonReceiveOrRespond With 4 Params
    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable? sut = null;

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), "test-name", () => { });

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.Empty, "test-name", () => { });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), name, () => { });

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), "test-name", null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateNonReceiveOrRespond With 5 Params
    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable? sut = null;

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), () => { });

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.Empty, () => { });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndActionAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable();

        // Act
        var act = () => sut.CreateNonReceiveOrRespond(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateOneWayReceive With 4 Params
    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, "test-name", _ => { });

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, "test-name", _ => { });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.NewGuid(), name, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.NewGuid(), "test-name", null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateOneWayReceive With 5 Params
    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, _ => { });

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.Empty, _ => { });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushReactable<int>();

        // Act
        var act = () => sut.CreateOneWayReceive(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceive')");
    }
    #endregion

    #region CreateOneWayRespond With 4 Params
    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPullReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, "test-name", () => 10);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, "test-name", () => 20);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.NewGuid(), name, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.NewGuid(), "test-name", null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onRespond')");
    }
    #endregion

    #region CreateOneWayRespond With 5 Params
    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPullReactable<int>? sut = null;

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, () => 30);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.Empty, () => 40);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoNameAndWithNullOnRespond_ThrowsException()
    {
        // Arrange
        var sut = new PullReactable<int>();

        // Act
        var act = () => sut.CreateOneWayRespond(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onRespond')");
    }
    #endregion

    #region CreateTwoWay With 4 Params
    [Fact]
    public void CreateTwoWay_WhenInvokingWithIdAndNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushPullReactable<int, bool>? sut = null;

        // Act
        var act = () => sut.CreateTwoWay(Guid.Empty, "test-name", _ => true);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateTwoWay_WhenInvokingWithIdAndNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushPullReactable<int, bool>();

        // Act
        var act = () => sut.CreateTwoWay(Guid.Empty, "test-name", _ => true);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'name')")]
    [InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
    public void CreateTwoWay_WhenInvokingWithIdAndNameAndWithNullOrEmptyName_ThrowsException(string? name, string expected)
    {
        // Arrange
        var sut = new PushPullReactable<int, bool>();

        // Act
        var act = () => sut.CreateTwoWay(Guid.NewGuid(), name, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expected);
    }

    [Fact]
    public void CreateTwoWay_WhenInvokingWithIdAndNameAndWithNullOnReceive_ThrowsException()
    {
        // Arrange
        var sut = new PushPullReactable<int, bool>();

        // Act
        var act = () => sut.CreateTwoWay(Guid.NewGuid(), "test-name", null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceiveRespond')");
    }
    #endregion

    #region CreateTwoWay With 5 Params
    [Fact]
    public void CreateTwoWay_WhenInvokingWithIdAndAutoNameAndWithNullReactable_ThrowsException()
    {
        // Arrange
        IPushPullReactable<int, bool>? sut = null;

        // Act
        var act = () => sut.CreateTwoWay(Guid.Empty, _ => true);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'reactable')");
    }

    [Fact]
    public void CreateTwoWay_WhenInvokingWithIdAndAutoNameAndWithEmptyId_ThrowsException()
    {
        // Arrange
        var sut = new PushPullReactable<int, bool>();

        // Act
        var act = () => sut.CreateTwoWay(Guid.Empty, _ => true);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The id cannot be empty. (Parameter 'id')");
    }

    [Fact]
    public void CreateTwoWay_WhenInvokingWithIdAndAutoNameAndWithNullOnRespond_ThrowsException()
    {
        // Arrange
        var sut = new PushPullReactable<int, bool>();

        // Act
        var act = () => sut.CreateTwoWay(Guid.NewGuid(), null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null. (Parameter 'onReceiveRespond')");
    }
    #endregion
    #endregion

    #region Method Non-Exception Tests
    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("45c55fd2-32c9-46de-bf24-65cffd2da87f");
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedNames = new[] { expectedName };
        var expectedAction = () => { };
        var sut = new PushReactable();

        // Act
        var unsubscriber = sut.CreateNonReceiveOrRespond(id, expectedName, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateNonReceiveOrRespond_WhenInvokingWithIdAndAutoName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("e743d8f1-3274-4cdf-af79-dba7b2356786");
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedName = $"{nameof(ReactableExtensionMethodsTests)}";
        expectedName += $".{nameof(CreateNonReceiveOrRespond_WhenInvokingWithIdAndAutoName_CreatesSubscription)}() - {id}";
        var expectedNames = new[] { expectedName };
        var expectedAction = () => { };
        var sut = new PushReactable();

        // Act
        var unsubscriber = sut.CreateNonReceiveOrRespond(id, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayReceive_WhenInvokingWithIdAndName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("ceb12e34-1dcc-400c-a42e-29f357fecfe6");
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedNames = new[] { expectedName };
        var expectedAction = (int _) => { };
        var sut = new PushReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayReceive(id, expectedName, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayReceive_WhenInvokingWithIdAndAutoName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("770a143e-9694-48e1-9ad6-db7f70224308");
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedName = $"{nameof(ReactableExtensionMethodsTests)}";
        expectedName += $".{nameof(CreateOneWayReceive_WhenInvokingWithIdAndAutoName_CreatesSubscription)}() - {id}";
        var expectedNames = new[] { expectedName };
        var expectedAction = (int _) => { };
        var sut = new PushReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayReceive(id, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayRespond_WhenInvokingWithIdAndName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("ef2b9247-9d9f-4efc-910e-2ae4727f49f3");
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedNames = new[] { expectedName };
        var expectedAction = () => 123;
        var sut = new PullReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayRespond(id, expectedName, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateOneWayRespond_WhenInvokingWithIdAndAutoName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("1201281a-dd5b-4b85-a732-7789104217c8");
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedName = $"{nameof(ReactableExtensionMethodsTests)}";
        expectedName += $".{nameof(CreateOneWayRespond_WhenInvokingWithIdAndAutoName_CreatesSubscription)}() - {id}";
        var expectedNames = new[] { expectedName };
        var expectedAction = () => 456;
        var sut = new PullReactable<int>();

        // Act
        var unsubscriber = sut.CreateOneWayRespond(id, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateTwoWay_WhenInvokingWithIdAndName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("77dcfe92-8b22-426b-94ab-54fb0cd298ee");
        var expectedIds = new[] { id }.AsReadOnly();
        const string expectedName = "test-name";
        var expectedNames = new[] { expectedName };
        Func<int, string> expectedAction = _ => "test-value";
        var sut = new PushPullReactable<int, string>();

        // Act
        var unsubscriber = sut.CreateTwoWay(id, expectedName, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory]
    [MemberData(nameof(SubscribeErrorData))]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction", Justification = "Not required for testing.")]
    public void CreateTwoWay_WhenInvokingWithIdAndAutoName_CreatesSubscription(Action? onUnsubscribe, Action<Exception>? onError)
    {
        // Arrange
        var id = new Guid("6aea4904-0e72-40bf-8d0e-bee23c8aea7b");
        var expectedIds = new[] { id }.AsReadOnly();
        var expectedName = $"{nameof(ReactableExtensionMethodsTests)}";
        expectedName += $".{nameof(CreateTwoWay_WhenInvokingWithIdAndAutoName_CreatesSubscription)}() - {id}";
        var expectedNames = new[] { expectedName };
        Func<int, string> expectedAction = _ => "test-value";
        var sut = new PushPullReactable<int, string>();

        // Act
        var unsubscriber = sut.CreateTwoWay(id, expectedAction, onUnsubscribe, onError);

        // Assert
        Assert.NotNull(unsubscriber);
        sut.SubscriptionIds.Should().BeEquivalentTo(expectedIds);
        sut.SubscriptionNames.Should().BeEquivalentTo(expectedNames);
    }
    #endregion
}
