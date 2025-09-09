namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using MK.Pool;

    public sealed class Entity : IPoolable
    {
        private readonly Dictionary<Type, IComponent> typeToComponents = new();

        internal int    Index { get; private set; }
        internal string Name  { get; private set; }

        internal IEnumerable<IComponent> Components => this.typeToComponents.Values;

        internal void OnCreate(int index, string name)
        {
            this.Index = index;
            this.Name  = name;
        }

        internal void OnDestroy()
        {
            this.typeToComponents.Clear();
        }

        internal bool HasComponent(Type type)
        {
            return this.typeToComponents.ContainsKey(type);
        }

        internal bool HasComponent<TComponent>() where TComponent : IComponent
        {
            return this.HasComponent(typeof(TComponent));
        }

        internal IComponent GetComponent(Type type)
        {
            return this.typeToComponents.GetValueOrDefault(type);
        }

        internal TComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            var component = this.GetComponent(typeof(TComponent));
            if (component == null)
            {
                return default;
            }

            return (TComponent)component;
        }

        internal bool AddComponent(IComponent component)
        {
            var type = component.GetType();
            if (this.HasComponent(type))
            {
                return false;
            }

            this.typeToComponents.Add(type, component);

            return true;
        }

        internal bool SetComponent(IComponent component)
        {
            var type = component.GetType();
            if (!this.HasComponent(type))
            {
                return false;
            }

            this.typeToComponents.Add(type, component);

            return true;
        }

        internal void RemoveComponent(Type type)
        {
            this.typeToComponents.Remove(type);
        }

        void IPoolable.OnSpawn()
        {
        }

        void IPoolable.OnRecycle()
        {
            this.typeToComponents.Clear();
            this.Index = -1;
            this.Name  = null;
        }
    }
}