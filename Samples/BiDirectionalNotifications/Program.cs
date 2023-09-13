// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedVariable
#pragma warning disable CS8509
using Carbonate.TwoWay;

var favoriteGetter = new PullReactable<string, string>();

var msgEventId = Guid.NewGuid(); // This is the ID used to identify the event

IDisposable unsubscriber = favoriteGetter.Subscribe(new RespondReactor<string, string>(
    respondId: msgEventId,
    name: "adder",
    onRespondData: (data) => data switch
        {
            "prog-lang" => "C#",
            "food" => "scotch eggs",
            "past-time" => "game development",
            "music" => "hard rock/metal",
        },
    onUnsubscribe: () => Console.WriteLine("Unsubscribed from notifications!"),
    onError: (ex) => Console.WriteLine($"Error: {ex.Message}")));

Console.WriteLine($"Favorite Language: {favoriteGetter.Pull("prog-lang", msgEventId)}");
Console.WriteLine($"Favorite Food: {favoriteGetter.Pull("food", msgEventId)}");
Console.WriteLine($"Favorite Past Time: {favoriteGetter.Pull("past-time", msgEventId)}");
Console.WriteLine($"Favorite Music: {favoriteGetter.Pull("music", msgEventId)}");
