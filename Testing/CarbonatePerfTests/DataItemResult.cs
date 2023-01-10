// <copyright file="DataItemResult.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

using Carbonate.Core;

public class DataItemResult : IResult<StructItem[]>
{
    private readonly StructItem[] dataItems;

    public DataItemResult(Memory<StructItem> dataItems) => this.dataItems = dataItems.ToArray();

    public bool IsEmpty => false;

    public StructItem[] GetValue(Action<Exception>? onError = null) => this.dataItems;
}
