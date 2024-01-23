// <copyright file="PushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

using System.Runtime.InteropServices;
using Core.NonDirectional;
using Exceptions;

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

                IsProcessing = true;
                subscription.OnReceive();
                IsProcessing = false;
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
    }
}
