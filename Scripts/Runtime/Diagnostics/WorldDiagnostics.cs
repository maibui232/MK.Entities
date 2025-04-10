#if UNITY_EDITOR_WIN
namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    [InitializeOnLoad]
    public static class WorldDiagnostics
    {
        static WorldDiagnostics()
        {
            EditorApplication.delayCall += () =>
                                           {
                                               EditorApplication.playModeStateChanged -= ClearWorld;
                                               EditorApplication.playModeStateChanged += ClearWorld;
                                           };

            return;
            void ClearWorld(PlayModeStateChange state) { Worlds.Clear(); }
        }

        public static Dictionary<Entity, EntityDiagnostics> EntityToDiagnostics = new();
        public static HashSet<World>                        Worlds { get; } = new();
        public static event Action<World>                   WorldAdded;
        public static event Action<World>                   WorldRemoved;

        internal static void AddWorld(World world)
        {
            if (!Worlds.Add(world)) return;
            WorldAdded?.Invoke(world);
        }

        internal static void RemoveWorld(World world)
        {
            if (!Worlds.Remove(world)) return;
            WorldRemoved?.Invoke(world);
        }
    }
}
#endif