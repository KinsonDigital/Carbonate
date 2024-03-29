﻿// <copyright file="SubscriptionBaseTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests;

using Carbonate.OneWay;
using FluentAssertions;
using Helpers.Fakes;
using Xunit;

public class SubscriptionBaseTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_SetsIdAndNameProps()
    {
        // Arrange & Act
        var id = Guid.NewGuid();
        var name = "test-name";
        var sut = new SubscriptionBaseFake(id, name);

        // Assert
        sut.Id.Should().Be(id);
        sut.Name.Should().Be(name);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void OnUnsubscribe_WhenNotUnsubscribed_InvokesAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnUnsubscribe() => onReceiveInvoked = true;

        var sut = new SubscriptionBaseFake(Guid.NewGuid(), onUnsubscribe: OnUnsubscribe);

        // Act
        sut.OnUnsubscribe();

        // Assert
        onReceiveInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnUnsubscribe_WhenUnsubscribed_DoesNotInvokeActionAgain()
    {
        // Arrange
        var totalInvokes = 0;
        void OnUnsubscribe() => totalInvokes += 1;

        var sut = new ReceiveSubscription<int>(Guid.NewGuid(), _ => { }, onUnsubscribe: OnUnsubscribe);
        sut.OnUnsubscribe();

        // Act
        sut.OnUnsubscribe();

        // Assert
        totalInvokes.Should().Be(1);
    }

    [Fact]
    public void OnError_WithNullErrorParameter_ThrowsException()
    {
        // Arrange
        var sut = new ReceiveSubscription<int>(Guid.NewGuid(), _ => { }, onError: _ => { });

        // Act
        var act = () => sut.OnError(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'error')");
    }

    [Fact]
    public void OnError_WhenNotUnsubscribed_InvokesAction()
    {
        // Arrange
        var onErrorInvoked = false;
        void OnError(Exception ex) => onErrorInvoked = true;

        var exception = new Exception("test-exception");

        var sut = new ReceiveSubscription<int>(Guid.NewGuid(), _ => { }, onError: OnError);

        // Act
        sut.OnError(exception);

        // Assert
        onErrorInvoked.Should().BeTrue();
    }

    [Fact]
    public void OnError_WhenNotSubscribed_DoesNotInvokeAction()
    {
        // Arrange
        var onReceiveInvoked = false;
        void OnError(Exception ex) => onReceiveInvoked = true;

        var exception = new Exception("test-exception");

        var sut = new ReceiveSubscription<int>(Guid.NewGuid(), _ => { }, onError: OnError);

        sut.OnUnsubscribe();

        // Act
        sut.OnError(exception);

        // Assert
        onReceiveInvoked.Should().BeFalse();
    }
    #endregion
}
