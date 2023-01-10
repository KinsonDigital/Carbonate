// <copyright file="DataResult.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

using Carbonate.Core;

public class DataResult : IResult<nint>
{
    private readonly nint dataItems;

    public DataResult(nint dataItems) => this.dataItems = dataItems;

    public bool IsEmpty => false;

    public nint GetValue(Action<Exception>? onError = null) => this.dataItems;
}
