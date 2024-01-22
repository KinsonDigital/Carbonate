// <copyright file="PushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using System.Runtime.InteropServices;
using Core.OneWay;

/// <inheritdoc cref="IPushReactable{T}"/>
public class PushReactable<TIn>
    : ReactableBase<IReceiveSubscription<TIn>>, IPushReactable<TIn>
{
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public void Push(Guid id, in TIn data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data), "The parameter must not be null.");
        }

        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(PushReactable<TIn>), $"{nameof(PushReactable<TIn>)} disposed.");
        }

        try
        {
            foreach (var subscription in CollectionsMarshal.AsSpan(InternalSubscriptions))
            {
                if (subscription.Id != id)
                {
                    continue;
                }

                subscription.OnReceive(data);
            }
        }
        catch (Exception e)
        {
            SendError(e, id);
        }
    }
}
