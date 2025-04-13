#if UNITY_EDITOR
namespace MK.Entities
{
    using System.Collections.Generic;

    public static class WorldDiagnostic
    {
        public static List<World> Worlds { get; } = new();

        internal static void AddWorld(World world) { Worlds.Add(world); }

        internal static void RemoveWorld(World world) { Worlds.Remove(world); }
    }
}
#endif