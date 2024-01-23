// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
#pragma warning disable SA1200
using System.Runtime.InteropServices;
using Samples;
#pragma warning restore SA1200

// Check if the current platform is windows
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    // Set the console window size to be larger than the default
    Console.SetWindowSize(Console.WindowWidth, Console.LargestWindowHeight);
}

var samples = new Dictionary<string, Action>
{
    ["Non-Directional Notifications Without Fluent Api"] = new NonDirectionalWithoutFluentApi().Run,
    ["One Way Notifications Without Fluent Api"] = new OneWayWithoutFluentApiSample().Run,
    ["Two Way Notifications Without Fluent Api"] = new TwoWayWithoutFluentApi().Run,
};

for (var i = 0; i < samples.Count; i++)
{
    var title = samples.ElementAt(i).Key;
    Console.WriteLine();
    var consoleClr = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"{i + 1}. {title}");
    Console.ForegroundColor = consoleClr;

    // Execute the sample
    samples.ElementAt(i).Value();

    Console.WriteLine();

    if (i < samples.Count - 1)
    {
        Console.WriteLine("Press any key to move to the next sample . . .");
        Console.WriteLine();

        consoleClr = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("-------------------------------------------------");
        Console.ForegroundColor = consoleClr;

        Console.ReadKey();
    }
}

Console.WriteLine();
Console.WriteLine("Running samples complete!!");
Console.ReadKey();
