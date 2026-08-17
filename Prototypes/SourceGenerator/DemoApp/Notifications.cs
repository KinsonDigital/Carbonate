// <copyright file="Notifications.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace DemoApp;

using Carbonate.Messaging;

/// <summary>
/// THE ONLY HAND-WRITTEN MESSAGE CODE.
/// Each one-line partial property declares a complete message contract.
/// The generator implements the property (see generated\...\Notifications.Messages.g.cs after build).
/// </summary>
[MessageHub]
public static partial class Notifications
{
    /// <summary>Non-directional: something happened, no data.</summary>
    public static partial Event BatchHasBegun { get; }

    /// <summary>One-way push: mouse state flows to subscribers.</summary>
    public static partial Event<MouseStateData> MouseStateChanged { get; }

    /// <summary>One-way pull: ask for the current window size.</summary>
    public static partial Request<WindowSizeData> GetWindowSize { get; }

    /// <summary>Two-way: send a question, get an answer back.</summary>
    public static partial Request<string, string> AskFavorite { get; }
}

/// <summary>Example payload record (as in Velaptor's ReactableData folder).</summary>
public readonly record struct MouseStateData(int X, int Y, bool LeftButtonDown);

/// <summary>Example payload record.</summary>
public readonly record struct WindowSizeData(int Width, int Height);
