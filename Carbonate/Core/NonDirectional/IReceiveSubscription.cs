// <copyright file="IReceiveSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantTypeDeclarationBody
namespace Carbonate.Core.NonDirectional;

/// <summary>
/// A subscription capable of standard functionality that can receive push notifications.
/// </summary>
public interface IReceiveSubscription : ISubscription, IReceiver
{
}
