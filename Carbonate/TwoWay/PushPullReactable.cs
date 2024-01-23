// <copyright file="PushPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// NOTE: Leave the loops as 'for loops'. This is a small performance improvement.
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
namespace Carbonate.TwoWay;

using System.Runtime.InteropServices;
using Core.TwoWay;
using Exceptions;

/// <inheritdoc cref="IPushPullReactable{TIn,TOut}"/>
public class PushPullReactable<TIn, TOut> : ReactableBase<IReceiveRespondSubscription<TIn, TOut>>, IPushPullReactable<TIn, TOut>
{
    /// <inheritdoc/>
    public TOut? PushPull(Guid id, in TIn data)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushPullReactable<TIn, TOut>), $"{nameof(PushPullReactable<TIn, TOut>)} disposed.");
        }

        try
        {
            foreach (var subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
            {
                if (subscription.Id != id)
                {
                    continue;
                }

                IsProcessing = true;
                var value = subscription.OnRespond(data) ?? default(TOut);
                IsProcessing = false;

                return value;
            }
        }
        catch (Exception e) when (e is NotificationException)
        {
            throw;
        }
        catch (Exception e)
        {
            SendError(e, id);
        }

        return default;
    }
}
