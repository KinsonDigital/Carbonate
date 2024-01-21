// <copyright file="PushPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.TwoWay;

using System.Runtime.InteropServices;
using Core.TwoWay;

/// <inheritdoc cref="IPushPullReactable{TIn,TOut}"/>
public class PushPullReactable<TIn, TOut> : ReactableBase<IReceiveRespondSubscription<TIn, TOut>>, IPushPullReactable<TIn, TOut>
{
    /// <inheritdoc/>
    public TOut? PushPull(in TIn data, Guid id)
    {
        try
        {
            foreach (var subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
            {
                if (subscription.Id != id)
                {
                    continue;
                }

                return subscription.OnRespond(data) ?? default(TOut);
            }
        }
        catch (Exception e)
        {
            SendError(e, id);
        }

        return default;
    }
}
