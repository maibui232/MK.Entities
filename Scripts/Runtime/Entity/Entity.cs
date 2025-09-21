namespace MK.Entities
{
    using System;

    public readonly struct Entity : IEquatable<Entity>
    {
        internal readonly int    Id;
        internal readonly string Name;

        public Entity(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        bool IEquatable<Entity>.Equals(Entity other)
        {
            return this.Id == other.Id && this.Name.Equals(other.Name);
        }
    }
}