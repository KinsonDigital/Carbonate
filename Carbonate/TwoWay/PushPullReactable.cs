// <copyright file="PushPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.TwoWay;

using Core.TwoWay;

/// <inheritdoc cref="IPushPullReactable{TDataIn,TDataOut}"/>
public class PushPullReactable<TDataIn, TDataOut> : ReactableBase<IRespondReactor<TDataIn, TDataOut>>, IPushPullReactable<TDataIn, TDataOut>
{
    /// <inheritdoc/>
    public TDataOut? PushPull(in TDataIn data, Guid respondId)
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
