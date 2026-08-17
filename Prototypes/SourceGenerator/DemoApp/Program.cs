// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using Carbonate.Messaging;
using Carbonate.NonDirectional;
using Carbonate.OneWay;
using Carbonate.TwoWay;
using DemoApp;

Console.WriteLine("=== Carbonate v2 message-centric prototype ===");
Console.WriteLine();

// Stable, deterministic IDs assigned by the generator - no hand-written Guids anywhere.
Console.WriteLine($"Message IDs (deterministic):");
Console.WriteLine($"  BatchHasBegun      -> {Notifications.BatchHasBegun.Id}");
Console.WriteLine($"  MouseStateChanged  -> {Notifications.MouseStateChanged.Id}");
Console.WriteLine($"  GetWindowSize      -> {Notifications.GetWindowSize.Id}");
Console.WriteLine($"  AskFavorite        -> {Notifications.AskFavorite.Id}");
Console.WriteLine();

// ----- Non-directional -----
var batchBus = new PushReactable();
using IDisposable batchSub = batchBus.Subscribe(Notifications.BatchHasBegun, () => Console.WriteLine("[sub] Batch has begun!"));
batchBus.Push(Notifications.BatchHasBegun);

// ----- One-way push -----
var mouseBus = new PushReactable<MouseStateData>();
using IDisposable mouseSub = mouseBus.Subscribe(Notifications.MouseStateChanged, state => Console.WriteLine($"[sub] Mouse at ({state.X}, {state.Y}), left button: {state.LeftButtonDown}"));
mouseBus.Push(Notifications.MouseStateChanged, new MouseStateData(120, 240, true));

// ----- One-way pull -----
var windowBus = new PullReactable<WindowSizeData>();
using IDisposable windowSub = windowBus.Respond(Notifications.GetWindowSize, () => new WindowSizeData(1920, 1080));
WindowSizeData size = windowBus.Pull(Notifications.GetWindowSize);
Console.WriteLine($"[pull] Window size: {size.Width}x{size.Height}");

// ----- Two-way -----
var favoritesBus = new PushPullReactable<string, string>();
using IDisposable favSub = favoritesBus.Respond(Notifications.AskFavorite, topic => topic switch
{
    "prog-lang" => "C#",
    "food" => "scotch eggs",
    _ => "unknown",
});
Console.WriteLine($"[pushpull] Favorite Language: {favoritesBus.PushPull(Notifications.AskFavorite, "prog-lang")}");
Console.WriteLine($"[pushpull] Favorite Food:     {favoritesBus.PushPull(Notifications.AskFavorite, "food")}");

Console.WriteLine();
Console.WriteLine("=== Done. Inspect generated code under DemoApp\\generated\\ ===");

// ----------------------------------------------------------------------------
// COMPILE-TIME SAFETY DEMO - uncomment any of these and the build FAILS:
//
// mouseBus.Push(Notifications.MouseStateChanged, "a string");     // wrong payload type
// batchBus.Push(Notifications.MouseStateChanged);                 // wrong message for this bus
// mouseBus.Push(Notifications.BatchHasBegun, new MouseStateData(1, 2, false)); // Event vs Event<T>
// ----------------------------------------------------------------------------
