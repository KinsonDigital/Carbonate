// <copyright file="IWithIdStage.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Fluent;

public interface IWithIdStage<out TResult>
{
    public TResult WithId(Guid id);
}
