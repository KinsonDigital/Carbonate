// <copyright file="PushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using Core.OneWay;

/// <inheritdoc cref="IPushReactable{T}"/>
public class PushReactable<TIn>
    : ReactableBase<IReceiveSubscription<TIn>>, IPushReactable<TIn>
{
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException">Thrown if this method is invoked after disposal.</exception>
    public void Push(in TIn data, Guid id)
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
            /* Work from the end to the beginning of the list
             * just in case the reactable is disposed(removed)
             * in the OnReceive() method.
             */
            for (var i = InternalSubscriptions.Count - 1; i >= 0; i--)
            {
                /*NOTE:
                 * The purpose of this logic is to prevent array index errors
                 * if an OnReceive() implementation ends up unsubscribing a single
                 * subscription or unsubscribing from a single event id
                 *
                 * If the current index is not less than or equal to
                 * the total number of items, reset the index to the last item
                 */
                i = i > InternalSubscriptions.Count - 1
                    ? InternalSubscriptions.Count - 1
                    : i;

                if (InternalSubscriptions[i].Id != id)
                {
                    continue;
                }

                InternalSubscriptions[i].OnReceive(data);
            }
        }
        catch (Exception e)
        {
            SendError(e, id);
        }
    }

    /// <summary>
    /// Sends an error to all of the subscribers that match the given <paramref name="id"/>.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="id">The ID of the event where the notification will be pushed.</param>
    private void SendError(Exception exception, Guid id)
    {
        /* Work from the end to the beginning of the list
         * just in case the reactable is disposed(removed)
         * in the OnReceive() method.
         */
        for (var i = InternalSubscriptions.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > InternalSubscriptions.Count - 1
                ? InternalSubscriptions.Count - 1
                : i;

            if (InternalSubscriptions[i].Id != id)
            {
                continue;
            }

            InternalSubscriptions[i].OnError(exception);
        }
    }
}
