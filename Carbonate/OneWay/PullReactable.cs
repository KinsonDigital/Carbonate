// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.OneWay;

using Core.OneWay;

/// <inheritdoc cref="IPullReactable{TDataOut}"/>
public class PullReactable<TDataOut>
    : ReactableBase<IRespondReactor<TDataOut>>, IPullReactable<TDataOut>
{
    /// <inheritdoc/>
    public TDataOut? Pull(Guid respondId)
    {
        for (var i = 0; i < Reactors.Count; i++)
        {
            if (Reactors[i].Id != respondId)
            {
                continue;
            }

            return Reactors[i].OnRespond() ?? default(TDataOut);
        }

        return default;
    }
}
