namespace MK.Entities
{
    using System;

    public readonly struct Entity : IEquatable<Entity>
    {
        internal readonly int Id;

        public Entity(int id)
        {
            this.Id = id;
        }

        bool IEquatable<Entity>.Equals(Entity other)
        {
            return this.Id == other.Id;
        }

        public override string ToString()
        {
            return $"Entity-{this.Id}";
        }
    }
}