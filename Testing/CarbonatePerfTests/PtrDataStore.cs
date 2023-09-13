// <copyright file="PtrDataStore.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

using System.Runtime.CompilerServices;
using Carbonate.OneWay;

public class PtrDataStore
{
    private Memory<StructItem> dataItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="PtrDataStore"/> class.
    /// </summary>
    /// <param name="pullReactable">Pulls data from a source.</param>
    public PtrDataStore(IPullReactable<nint> pullReactable)
    {
        var newDataItems = new StructItem[4];
        newDataItems[0] = new StructItem { NumberValue = 10, StringValue = "ten" };
        newDataItems[1] = new StructItem { NumberValue = 20, StringValue = "twenty" };
        newDataItems[2] = new StructItem { NumberValue = 30, StringValue = "thirty" };
        newDataItems[3] = new StructItem { NumberValue = 40, StringValue = "forty" };

        this.dataItems = new Memory<StructItem>(newDataItems);

        pullReactable.Subscribe(new RespondReactor<nint>(
            respondId: Ids.GetDatId,
            onRespond: GetDataPtr));
    }

    /// <summary>
    /// Gets the pointer data result.
    /// </summary>
    /// <returns>The result.</returns>
    private nint GetDataPtr()
    {
        unsafe
        {
            return new nint(Unsafe.AsPointer(ref this.dataItems));
        }
    }
}
