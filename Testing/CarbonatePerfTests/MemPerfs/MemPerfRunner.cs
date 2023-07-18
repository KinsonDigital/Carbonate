// <copyright file="MemPerfRunner.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests.MemPerfs;

using System.Diagnostics.CodeAnalysis;
using Carbonate.UniDirectional;

public static class MemPerfRunner
{
    // ReSharper disable NotAccessedField.Local
    private static readonly StructDataStore StructStructDataStore;
    private static readonly StructDataPuller StructStructDataPuller;
    private static readonly PtrDataStore PointerDataStore;
    private static readonly PtrDataPuller PointerDataPuller;

    // ReSharper restore NotAccessedField.Local

    /// <summary>
    /// Initializes static members of the <see cref="MemPerfRunner"/> class.
    /// </summary>
    static MemPerfRunner()
    {
        var pullStructReactable = new PullReactable<StructItem[]>();
        StructStructDataStore = new StructDataStore(pullStructReactable);
        StructStructDataPuller = new StructDataPuller(pullStructReactable);

        var pullPtrReactable = new PullReactable<nint>();
        PointerDataStore = new PtrDataStore(pullPtrReactable);
        PointerDataPuller = new PtrDataPuller(pullPtrReactable);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used for testing.")]
    public static void Run(PerfScenarios perfScenarioTypes, int iterationTime = -1)
    {
        while (true)
        {
            switch (perfScenarioTypes)
            {
                case PerfScenarios.PullReactable_Pull_Method_With_Struct:
                    _ = StructStructDataPuller.Pull();
                    break;
                case PerfScenarios.PullReactable_Pull_Method_With_Ptr:
                    _ = PointerDataPuller.Pull();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(perfScenarioTypes), perfScenarioTypes, null);
            }

            if (iterationTime != -1)
            {
                Thread.Sleep(iterationTime);
            }
        }
    }
}
