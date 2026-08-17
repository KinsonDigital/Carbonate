# Carbonate v2 Message-Centric API — Source Generator Prototype

A working spike of the design proposed in [issue #275](https://github.com/KinsonDigital/Carbonate/issues/275).
It demonstrates that a Roslyn incremental source generator can implement one-line
message declarations with deterministic IDs, wire them to the **real Carbonate v1**
reactables with full compile-time type safety, and emit a communication manifest.

## Layout

| Project | Purpose | Who writes it in v2 |
|---|---|---|
| `Carbonate.Messaging` | `Event` / `Event<T>` / `Request<T>` / `Request<TIn,TOut>` contract types + typed `Push`/`Subscribe`/`Pull`/`Respond` extensions over the real v1 reactables | Carbonate library (once) |
| `Carbonate.Generators` | The incremental generator (`MessageHubGenerator`) | Carbonate library (once) |
| `DemoApp` | A consumer: declares a `Notifications` hub and uses it | **You** (1 line per message) |

## Try it

```powershell
dotnet build Prototypes\SourceGenerator\DemoApp\DemoApp.csproj
dotnet run --project Prototypes\SourceGenerator\DemoApp\DemoApp.csproj
```

## See the generated code

The DemoApp sets `EmitCompilerGeneratedFiles`, so after building, the
generator's output is on disk at:

```
DemoApp\generated\Carbonate.Generators\Carbonate.Generators.MessageHubGenerator\Notifications.Messages.g.cs
```

You'll see the other half of your `partial class` — the part you never write.
The manifest (`Carbonate.Manifest.json`) is also emitted into the compilation.

## Compile-time safety demo

Open `DemoApp\Program.cs` and uncomment any line in the "COMPILE-TIME SAFETY DEMO"
block at the bottom — each one pairs the wrong payload, wrong bus, or wrong message
kind, and each one fails the build. That's the entire point: the Guid↔payload↔bus
contract moves from convention to the compiler.
