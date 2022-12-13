// <copyright file="ObjectMessage.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTesting;

using Carbonate;

public class ObjectMessage : IMessage
{
    private IMyObject window;

    public ObjectMessage(IMyObject window) => this.window = window;

    public T? GetData<T>(Action<Exception>? onError = null)
        where T : class
    {
        try
        {
            return (T)this.window;
        }
        catch (Exception e)
        {
            if (onError is null)
            {
                throw;
            }

            onError.Invoke(e);
        }

        return null;
    }
}
