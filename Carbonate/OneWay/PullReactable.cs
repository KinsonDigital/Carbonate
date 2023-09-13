// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.OneWay;

using Core.OneWay;

/// <inheritdoc cref="IPullReactable{TOut}"/>
public class PullReactable<TOut>
    : ReactableBase<IRespondReactor<TOut>>, IPullReactable<TOut>
{
    /// <inheritdoc/>
    public TOut? Pull(Guid respondId)
    {
        for (var i = 0; i < Reactors.Count; i++)
        {
            if (Reactors[i].Id != respondId)
            {
                continue;
            }

            return Reactors[i].OnRespond() ?? default(TOut);
        }

        return default;
    }
}
