// <copyright file="PushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

using System.Runtime.InteropServices;
using Core.NonDirectional;

/// <inheritdoc cref="IPushReactable"/>
public class PushReactable : ReactableBase<IReceiveSubscription>, IPushReactable
{
    /// <inheritdoc cref="IPushable.Push"/>
    public void Push(Guid id)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable), $"{nameof(PushReactable)} disposed.");
        }

        try
        {
            foreach (var subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
            {
                if (subscription.Id != id)
                {
                    continue;
                }

                subscription.OnReceive();
            }
        }
        catch (Exception e)
        {
            SendError(e, id);
        }
    }
}
