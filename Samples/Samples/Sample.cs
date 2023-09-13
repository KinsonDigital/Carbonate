// <copyright file="Sample.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Samples;

/// <summary>
/// Represents a sample.
/// </summary>
public abstract class Sample
{
    /// <summary>
    /// Runs the sample.
    /// </summary>
    public abstract void Run();

    /// <summary>
    /// Prints a description of the sample to the console.
    /// </summary>
    /// <param name="description">The description to print.</param>
    protected void PrintDescription(string description)
    {
        Console.WriteLine();
        Console.WriteLine(description);
        Console.WriteLine();
    }
}
