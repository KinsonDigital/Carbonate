// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1200
#pragma warning disable CA1852

using Carbonate;
using CarbonateTesting;

var serializer = new DataSerializer();

var reactable = new Reactable(serializer);

var eventId = Guid.NewGuid();

// reactable.Subscribe(new Reactor(
//     eventId,
//     onNext: msg =>
//     {
//         var data = msg.GetData<SunData>();
//
//         Console.WriteLine("Received Sun Data:");
//         Console.WriteLine($"\tCircumference: {data.Circumference:N0}km");
//         Console.WriteLine($"\tRadius: {data.Radius:N0}km");
//         Console.WriteLine($"\tStar Type: {data.StarType}");
//     }));

// var data = new SunData
// {
//     Circumference = 4_379_000,
//     Radius = 695_700,
//     StarType = "Yellow Dwarf",
// };
//

// reactable.Push(data, eventId);
var window = new MyObject(123, 456);
reactable.Subscribe(new Reactor(
    eventId,
    onNext: msg =>
    {
        var isError = false;
        var winData = msg.GetData<IMyObject>(e =>
        {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {e.Message}");
            Console.ForegroundColor = clr;
            isError = true;
        });

        if (isError is false)
        {
            Console.WriteLine($"Obj Values: Value-1:{winData.Value1}, Value-2:{winData.Value2}");
        }
    }));

var glContextMsg = new ObjectMessage(window);

reactable.Push(glContextMsg, eventId);

Console.ReadLine();
