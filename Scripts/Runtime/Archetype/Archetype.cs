namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal readonly struct Archetype
    {
        private readonly IReadOnlyCollection<Type>                        componentTypes;
        private readonly Dictionary<Entity, Dictionary<Type, IComponent>> idToComponentLookup;

        public Archetype(IReadOnlyCollection<Type> componentTypes)
        {
            this.componentTypes      = componentTypes;
            this.idToComponentLookup = new Dictionary<Entity, Dictionary<Type, IComponent>>();
        }

        private string StatisticName => $"Archetype:{string.Join('-', this.componentTypes.Select(type => type.Name))}";

        internal bool Contains(Entity entity)
        {
            return this.idToComponentLookup.ContainsKey(entity);
        }

        internal IReadOnlyCollection<IComponent> GetComponents(Entity entity)
        {
            if (!this.idToComponentLookup.TryGetValue(entity, out var componentLookup))
            {
                throw new InvalidOperationException($"{entity} doesn't contain in archetype {this.StatisticName}");
            }

            return componentLookup.Values;
        }

        internal IComponent GetComponent(Entity entity, Type componentType)
        {
            if (!this.idToComponentLookup.TryGetValue(entity, out var componentLookup))
            {
                throw new InvalidOperationException($"{entity} doesn't contain in archetype {this.StatisticName}");
            }

            if (componentLookup.TryGetValue(componentType, out var component)) return component;

            throw new InvalidOperationException($"{entity} doesn't contain component of type {componentType.FullName}");
        }

        internal T GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            return (T)this.GetComponent(entity, typeof(T));
        }

        internal void SetComponent(Entity entity, IComponent component)
        {
            if (!this.idToComponentLookup.TryGetValue(entity, out var componentLookup))
            {
                throw new InvalidOperationException($"{entity} doesn't contain in archetype {this.StatisticName}");
            }

            var type = component.GetType();
            if (!componentLookup.ContainsKey(type))
            {
                throw new InvalidOperationException($"Component of type {type.FullName} doesn't contain in archetype {this.StatisticName}");
            }

            componentLookup[type] = component;
        }

        internal void AddEntity(Entity entity, IReadOnlyCollection<IComponent> components)
        {
            if (this.idToComponentLookup.ContainsKey(entity))
            {
                throw new Exception($"Entity {entity} is already added into {this.StatisticName}.");
            }

            var componentLookup = new Dictionary<Type, IComponent>();
            this.idToComponentLookup[entity] = componentLookup;
            foreach (var component in components)
            {
                componentLookup.Add(component.GetType(), component);
            }
        }

        internal void RemoveEntity(Entity entity)
        {
            if (!this.idToComponentLookup.Remove(entity))
            {
                throw new Exception($"Entity {entity} is already removed from {this.StatisticName}.");
            }
        }
    }
}