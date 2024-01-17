// <copyright file="PushPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.TwoWay;

using Core.TwoWay;

/// <inheritdoc cref="IPushPullReactable{TIn,TOut}"/>
public class PushPullReactable<TIn, TOut> : ReactableBase<IReceiveRespondSubscription<TIn, TOut>>, IPushPullReactable<TIn, TOut>
{
    /// <inheritdoc/>
    public TOut? PushPull(in TIn data, Guid respondId)
    {
        for (var i = 0; i < Subscriptions.Count; i++)
        {
            if (Subscriptions[i].Id != respondId)
            {
                continue;
            }

            return Subscriptions[i].OnRespond(data);
        }

        return default;
    }
}
