// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using System.Runtime.InteropServices;
using Core.OneWay;

/// <inheritdoc cref="IPullReactable{TOut}"/>
public class PullReactable<TOut>
    : ReactableBase<IRespondSubscription<TOut>>, IPullReactable<TOut>
{
    /// <inheritdoc/>
    public TOut? Pull(Guid id)
    {
        foreach (var subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
        {
            if (subscription.Id != id)
            {
                continue;
            }

            return subscription.OnRespond() ?? default(TOut);
        }

        return default;
    }
}
