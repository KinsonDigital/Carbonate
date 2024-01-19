// <copyright file="PushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

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
            /* Work from the end to the beginning of the list
             * just in case the reactable is disposed(removed)
             * in the OnReceive() method.
             */
            for (var i = Subscriptions.Count - 1; i >= 0; i--)
            {
                /*NOTE:
                 * The purpose of this logic is to prevent array index errors
                 * if an OnReceive() implementation ends up unsubscribing a single
                 * subscription or unsubscribing from a single event id
                 *
                 * If the current index is not less than or equal to
                 * the total number of items, reset the index to the last item
                 */
                i = i > Subscriptions.Count - 1
                    ? Subscriptions.Count - 1
                    : i;

                if (Subscriptions[i].Id != id)
                {
                    continue;
                }

                Subscriptions[i].OnReceive();
            }
        }
        catch (Exception e)
        {
            SendError(e, eventId);
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
        for (var i = Subscriptions.Count - 1; i >= 0; i--)
        {
            /*NOTE:
             * The purpose of this logic is to prevent array index errors
             * if an OnReceive() implementation ends up unsubscribing a single
             * subscription or unsubscribing from a single event id
             *
             * If the current index is not less than or equal to
             * the total number of items, reset the index to the last item
             */
            i = i > Subscriptions.Count - 1
                ? Subscriptions.Count - 1
                : i;

            if (Subscriptions[i].Id != id)
            {
                continue;
            }

            Subscriptions[i].OnError(exception);
        }
    }
}
