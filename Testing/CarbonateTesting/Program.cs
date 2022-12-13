// See https://aka.ms/new-console-template for more information


using Carbonate;
using CarbonateTesting;

var serializer = new DataSerializer();

var reactable = new Reactable(serializer);

var eventId = Guid.NewGuid();

reactable.Subscribe(new Reactor(
    eventId,
    onNext: msg =>
    {
        var data = msg.Deserialize<SunData>();

        Console.WriteLine("Received Sun Data:");
        Console.WriteLine($"\tCircumference: {data.Circumference:N0}km");
        Console.WriteLine($"\tRadius: {data.Radius:N0}km");
        Console.WriteLine($"\tStar Type: {data.StarType}");
    }));

var data = new SunData
{
    Circumference = 4_379_000,
    Radius = 695_700,
    StarType = "Yellow Dwarf",
};

reactable.Push(data, eventId);

Console.ReadLine();

