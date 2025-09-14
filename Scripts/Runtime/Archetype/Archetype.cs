namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MK.Pool;

    public sealed class Archetype : IPoolable<IEnumerable<Type>>
    {
        private          HashSet<Type>                        componentTypes;
        private readonly Dictionary<int, HashSet<IComponent>> idToComponents = new();

        public string StatisticName => $"Archetype:{string.Join('-', this.componentTypes.Select(type => type.Name))}";

        internal bool IsEmpty => this.idToComponents.Count == 0;

        internal bool Is(IEnumerable<Type> types) => types.All(type => this.componentTypes.Contains(type));

        internal bool Contains(Entity entity)
        {
            return this.idToComponents.ContainsKey(entity.Id);
        }

        internal void AddEntity(Entity entity, IEnumerable<IComponent> components)
        {
            if (this.idToComponents.ContainsKey(entity.Id))
            {
                throw new Exception($"Entity {entity.Id} is already added into {this.StatisticName}.");
            }

            this.idToComponents[entity.Id] = components.ToHashSet();
        }

        internal void RemoveEntity(Entity entity)
        {
            if (!this.idToComponents.Remove(entity.Id))
            {
                throw new Exception($"Entity {entity.Id} is already removed from {this.StatisticName}.");
            }
        }

        void IPoolable<IEnumerable<Type>>.OnSpawned(IEnumerable<Type> param)
        {
            this.componentTypes = param.ToHashSet();
        }

        void IRecyclable.OnRecycled()
        {
            this.componentTypes.Clear();
            this.idToComponents.Clear();
        }

        public sealed class ObjectPool : ObjectPool<Archetype, IEnumerable<Type>>
        {
            protected override Archetype Create(IEnumerable<Type> param) => new();
        }
    }
}