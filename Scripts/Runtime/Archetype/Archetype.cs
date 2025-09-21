namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Archetype
    {
        private readonly IReadOnlyCollection<Type>            componentTypes;
        private readonly Dictionary<int, HashSet<IComponent>> idToComponents = new();
        private readonly List<Entity>                         entities       = new();

        public Archetype(IReadOnlyCollection<Type> componentTypes)
        {
            this.componentTypes = componentTypes;
        }

        public string StatisticName => $"Archetype:{string.Join('-', this.componentTypes.Select(type => type.Name))}";

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
    }
}