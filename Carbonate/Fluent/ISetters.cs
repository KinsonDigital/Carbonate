// <copyright file="ISetters.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

public interface ISetters<out TResult>
{
    TResult WithName(string name);
    TResult WhenUnsubscribing(Action onUnsubscribe);
    TResult WithError(Action<Exception> onError);
}
