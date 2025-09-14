namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using MK.Pool;

    public sealed class Entity : IPoolable<Entity.Param>, IEquatable<Entity>
    {
#region ObjectPool

        public sealed class Param
        {
            public int    Id   { get; }
            public string Name { get; }

            public Param(int id, string name)
            {
                this.Id   = id;
                this.Name = name;
            }
        }

        internal sealed class ObjectPool : ObjectPool<Entity, Param>
        {
            protected override Entity Create(Param param) => new();
        }

        void IPoolable<Param>.OnSpawned(Param param)
        {
            this.Id   = param.Id;
            this.Name = param.Name;
        }

        void IRecyclable.OnRecycled()
        {
            this.Id   = -1;
            this.Name = string.Empty;
        }

#endregion

#region Fields

        private readonly Dictionary<Type, IComponent> typeToComponents = new();

        internal int    Id   { get; private set; }
        internal string Name { get; private set; }

#endregion

#region IEquatable

        bool IEquatable<Entity>.Equals(Entity other)
        {
            return other != null && this.Id == other.Id;
        }

#endregion

#region Internal Methods

        internal IReadOnlyDictionary<Type, IComponent> TypeToComponents => this.typeToComponents;

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

#endregion
    }
}