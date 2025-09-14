namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using MK.Extensions;
    using Enumerable = System.Linq.Enumerable;

    internal sealed class ArchetypeKey : IEquatable<ArchetypeKey>
    {
        private readonly Type[] types;

        internal IReadOnlyCollection<Type> Types => this.types;

        public ArchetypeKey(params Type[] types)
        {
            this.types = types.Sort();
        }

        bool IEquatable<ArchetypeKey>.Equals(ArchetypeKey other)
        {
            if (other == null || this.types.Length != other.types.Length) return false;
            return !Enumerable.Any(Enumerable.Where(this.types, (t, i) => t != other.types[i]));
        }
    }
}