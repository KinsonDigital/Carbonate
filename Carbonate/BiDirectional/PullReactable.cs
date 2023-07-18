// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.BiDirectional;

using Core.BiDirectional;

/// <inheritdoc cref="IPullReactable{TDataIn,TDataOut}"/>
public class PullReactable<TDataIn, TDataOut>
    : ReactableBase<IRespondReactor<TDataIn, TDataOut>>, IPullReactable<TDataIn, TDataOut>
{
    /// <inheritdoc/>
    public TDataOut? Pull(in TDataIn data, Guid respondId)
    {
        for (var i = 0; i < Reactors.Count; i++)
        {
            if (Reactors[i].Id != respondId)
            {
                continue;
            }

            return Reactors[i].OnRespond(data);
        }

        return default;
    }
}
