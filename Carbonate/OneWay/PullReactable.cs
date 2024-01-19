// <copyright file="PullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using Core.OneWay;

/// <inheritdoc cref="IPullReactable{TOut}"/>
public class PullReactable<TOut>
    : ReactableBase<IRespondSubscription<TOut>>, IPullReactable<TOut>
{
    /// <inheritdoc/>
    public TOut? Pull(Guid id)
    {
        for (var i = 0; i < InternalSubscriptions.Count; i++)
        {
            if (InternalSubscriptions[i].Id != id)
            {
                continue;
            }

            return InternalSubscriptions[i].OnRespond() ?? default(TOut);
        }

        return default;
    }
}
