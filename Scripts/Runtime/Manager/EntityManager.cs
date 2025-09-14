namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MK.Pool;
    using UnityEngine;

    public sealed class EntityManager
    {
        private readonly IObjectPool<Entity, Entity.Param>         entityObjectPool    = new Entity.ObjectPool();
        private readonly IObjectPool<Archetype, IEnumerable<Type>> archetypeObjectPool = new Archetype.ObjectPool();
        private readonly Dictionary<ArchetypeKey, Archetype>       keyToArchetype      = new();
        private readonly EntityCommandBuffer                       ecb                 = new();

        public Entity CreateEntity()
        {
            var index  = this.entityObjectPool.InstanceSpawned.Count;
            var entity = this.entityObjectPool.Spawn(new Entity.Param(index, $"Entity-{index}"));

            return entity;
        }

#region Query

        private bool GetOrCreateArchetype(ArchetypeKey key, out Archetype archetype)
        {
            if (this.keyToArchetype.TryGetValue(key, out archetype)) return true;
            this.keyToArchetype[key] = archetype = this.archetypeObjectPool.Spawn(key.Types);

            return false;
        }

        private void OnAddedComponent(Entity entity, Type componentType)
        {
                
        }

        private void OnRemovedComponent(Entity entity, Type componentType)
        {
        }

        private void OnDestroyEntity(Entity entity)
        {
        }

        private Archetype Query(params Type[] types)
        {
            var key = new ArchetypeKey(types);
            if (this.GetOrCreateArchetype(key, out var archetype))
            {
                return archetype;
            }

            // filter entity
            var keyValuePairs = this.entityObjectPool.InstanceSpawned.Where(entity => types.All(type => entity.TypeToComponents.ContainsKey(type)));
            foreach (var entity in keyValuePairs)
            {
                archetype.AddEntity(entity, entity.TypeToComponents.Values);
            }

            return archetype;
        }

        public Archetype Query<T>() where T : IComponent
        {
            return this.Query(typeof(T));
        }
#endregion

#region Command Buffer

        internal void PlaybackECB()
        {
            this.ecb.Playback();

            // implement all through archetype
        }

        public void Link(Entity entity, GameObject obj)
        {
            this.ecb.Link(entity, obj);
        }

        public void Unlink(GameObject obj)
        {
            this.ecb.Unlink(obj);
        }

        public void AddComponent(Entity entity, IComponent component)
        {
            this.ecb.AddComponent(entity, component, () => this.OnAddedComponent(entity, component.GetType()));
        }

        public void AddComponent<TComponent>(Entity entity, TComponent component) where TComponent : IComponent
        {
            this.ecb.AddComponent(entity, component, () => this.OnAddedComponent(entity, component.GetType()));
        }

        public void SetComponent(Entity entity, IComponent component)
        {
            this.ecb.SetComponent(entity, component);
        }

        public void SetComponent<TComponent>(Entity entity, TComponent component) where TComponent : IComponent
        {
            this.ecb.SetComponent(entity, component);
        }

        public void RemoveComponent(Entity entity, Type type)
        {
            this.ecb.RemoveComponent(entity, type, () => this.OnRemovedComponent(entity, type));
        }

        public void RemoveComponent<TComponent>(Entity entity) where TComponent : IComponent
        {
            this.ecb.RemoveComponent(entity, typeof(TComponent), () => this.OnRemovedComponent(entity, typeof(TComponent)));
        }

        public void DestroyEntity(Entity entity)
        {
            this.ecb.DestroyEntity(entity, this.entityObjectPool, () => this.OnDestroyEntity(entity));
        }

#endregion
    }
}