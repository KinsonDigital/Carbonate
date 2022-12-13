// <copyright file="MyObject.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTesting;

public class MyObject : IMyObject
{
    public MyObject(int value1, int value2)
    {
        Value1 = value1;
        Value2 = value2;
    }

    public int Value1 { get; }

    public int Value2 { get; }
}
