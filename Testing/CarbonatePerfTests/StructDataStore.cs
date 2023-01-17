// <copyright file="StructDataStore.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

using Carbonate.UniDirectional;

/// <summary>
/// Used for performance testing.
/// </summary>
public class StructDataStore
{
    // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
    private readonly Memory<StructItem> dataItems;

    // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

    /// <summary>
    /// Initializes a new instance of the <see cref="StructDataStore"/> class.
    /// </summary>
    /// <param name="pullReactable">Pulls data from a source.</param>
    public StructDataStore(IPullReactable<StructItem[]> pullReactable)
    {
        var newDataItems = new StructItem[4];
        newDataItems[0] = new StructItem { NumberValue = 10, StringValue = "ten" };
        newDataItems[1] = new StructItem { NumberValue = 20, StringValue = "twenty" };
        newDataItems[2] = new StructItem { NumberValue = 30, StringValue = "thirty" };
        newDataItems[3] = new StructItem { NumberValue = 40, StringValue = "forty" };

        this.dataItems = new Memory<StructItem>(newDataItems);

        pullReactable.Subscribe(new RespondReactor<StructItem[]>(
            respondId: Ids.GetDatId,
            onRespond: () => this.dataItems.Span.ToArray()));
    }
}
