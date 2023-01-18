// <copyright file="DataResult.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

public class DataResult
{
    private readonly nint dataItems;

    public DataResult(nint dataItems) => this.dataItems = dataItems;

    public bool IsEmpty => false;

    public nint GetValue(Action<Exception>? onError = null) => this.dataItems;
}
