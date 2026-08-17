// <copyright file="MessageHubAttribute.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Messaging;

/// <summary>
/// Marks a <c>static partial class</c> as a message hub.
/// The Carbonate source generator implements every <c>partial</c> property
/// declared in the class with a stable, deterministic message instance.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MessageHubAttribute : Attribute;
