// <copyright file="EmptySubscriptionIdExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Exceptions;

using Carbonate.Exceptions;
using FluentAssertions;
using Xunit;

/// <summary>
/// Tests the <see cref="EmptySubscriptionIdException"/> class.
/// </summary>
public class EmptySubscriptionIdExceptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new EmptySubscriptionIdException();

        // Assert
        exception.Message.Should().Be("The subscription ID cannot be empty.");
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new EmptySubscriptionIdException("test-message");

        // Assert
        exception.Message.Should().Be("test-message");
    }

    [Fact]
    public void Ctor_WhenInvokedWithMessageAndInnerException_ThrowsException()
    {
        // Arrange
        var innerException = new Exception("inner-exception");

        // Act
        var deviceException = new EmptySubscriptionIdException("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
    }
    #endregion
}
