namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal readonly struct ArchetypeKey : IEquatable<ArchetypeKey>
    {
        private readonly int[]                     componentIds;
        public readonly  IReadOnlyCollection<Type> ComponentTypes;

        public ArchetypeKey(IReadOnlyCollection<Type> types)
        {
            this.ComponentTypes = types;
            this.componentIds   = types.Select(ArchetypeKeyExtensions.ComponentId).ToArray();
            Array.Sort(this.componentIds);
        }

        private static class ArchetypeKeyExtensions
        {
            private static readonly Dictionary<Type, int> TypeToId = new();

            public static int ComponentId(Type componentType)
            {
                if (!TypeToId.TryGetValue(componentType, out var id))
                {
                    TypeToId[componentType] = id = TypeToId.Count;
                }

                return id;
            }
        }

        bool IEquatable<ArchetypeKey>.Equals(ArchetypeKey other)
        {
            if (this.componentIds.Length != other.componentIds.Length) return false;
            return !this.componentIds.Where((t, i) => t != other.componentIds[i]).Any();
        }
    }
}