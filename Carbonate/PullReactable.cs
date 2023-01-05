// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Diagnostics.CodeAnalysis;
using Core;
using Services;

/// <inheritdoc cref="IPullReactable"/>
public sealed class PullReactable : ReactableBase<IRespondReactor>, IPullReactable
{
    private readonly ISerializerService serializerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PullReactable"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public PullReactable() => this.serializerService = new JsonSerializerService();

    /// <summary>
    /// Initializes a new instance of the <see cref="PullReactable"/> class.
    /// </summary>
    /// <param name="serializerService">The serializer used to deserialize the message.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="serializerService"/> is null.</exception>
    public PullReactable(ISerializerService serializerService) =>
        this.serializerService = serializerService ??
            throw new ArgumentNullException(nameof(serializerService), "The parameter must not be null.");

    /// <inheritdoc/>
    public IResult? Pull(Guid respondId)
    {
        foreach (var reactor in Reactors)
        {
            var result = reactor.OnRespond();

            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public IResult? Pull<T>(in T data, Guid respondId)
        where T : class
    {
        foreach (var reactor in Reactors)
        {
            var jsonData = this.serializerService.Serialize(data);
            var outgoingMsg = new JsonMessage(this.serializerService, jsonData);

            var result = reactor.OnRespond(outgoingMsg);

            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }
}
