// <copyright file="Messages.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Messaging;

/// <summary>
/// A non-directional message: notifies subscribers that something happened, with no data.
/// Declared once in a message hub; the source generator provides the implementation.
/// </summary>
public sealed class Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Event"/> class.
    /// Intended to be invoked by generated code only.
    /// </summary>
    /// <param name="id">The stable, generator-assigned message ID.</param>
    /// <param name="name">The declaring property name.</param>
    public Event(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>Gets the stable, generator-assigned ID of this message.</summary>
    public Guid Id { get; }

    /// <summary>Gets the human-readable name of this message (the declaring property name).</summary>
    public string Name { get; }
}

/// <summary>
/// A one-way push message: data flows from publisher to subscribers.
/// </summary>
/// <typeparam name="T">The type of data carried by this message.</typeparam>
public sealed class Event<T>
{
    /// <inheritdoc cref="Event.Event(Guid, string)"/>
    public Event(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <inheritdoc cref="Event.Id"/>
    public Guid Id { get; }

    /// <inheritdoc cref="Event.Name"/>
    public string Name { get; }
}

/// <summary>
/// A one-way pull message: the publisher asks subscribers for data.
/// </summary>
/// <typeparam name="TOut">The type of data returned by the subscriber.</typeparam>
public sealed class Request<TOut>
{
    /// <inheritdoc cref="Event.Event(Guid, string)"/>
    public Request(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <inheritdoc cref="Event.Id"/>
    public Guid Id { get; }

    /// <inheritdoc cref="Event.Name"/>
    public string Name { get; }
}

/// <summary>
/// A two-way message: data is pushed out and different data is returned.
/// </summary>
/// <typeparam name="TIn">The type of data sent to the subscriber.</typeparam>
/// <typeparam name="TOut">The type of data returned by the subscriber.</typeparam>
public sealed class Request<TIn, TOut>
{
    /// <inheritdoc cref="Event.Event(Guid, string)"/>
    public Request(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <inheritdoc cref="Event.Id"/>
    public Guid Id { get; }

    /// <inheritdoc cref="Event.Name"/>
    public string Name { get; }
}
