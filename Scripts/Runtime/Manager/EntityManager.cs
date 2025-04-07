namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using global::MK.Factory;
    using UnityEngine;

    public class EntityManager
    {
        private readonly IFactory<Entity> entityFactory;

        private readonly HashSet<Entity>                entities              = new();
        private readonly EntityCommandBuffer            ecb                   = new();
        private readonly List<ICollector>               collectors            = new();
        private readonly Dictionary<GameObject, Entity> gameObjToLinkedEntity = new();

        public EntityManager(IFactory<Entity> entityFactory)
        {
            this.entityFactory = entityFactory;
        }

        public Entity CreateEntity()
        {
            var index  = this.entities.Count;
            var entity = this.entityFactory.Instantiate();
            entity.OnCreate(index, $"Entity-{index}");
            this.entities.Add(entity);

            return entity;
        }

#region Command Buffer

        internal void PlaybackECB()
        {
            this.ecb.Playback();
            foreach (var collector in this.collectors)
            {
                collector.Collect(this.entities);
            }
        }

        public void Link(Entity entity, GameObject obj)
        {
            if (this.gameObjToLinkedEntity.ContainsKey(obj))
            {
                return;
            }

            this.ecb.Link(entity, obj);
            this.gameObjToLinkedEntity.Add(obj, entity);
        }

        public void Unlink(GameObject obj)
        {
            if (!this.gameObjToLinkedEntity.Remove(obj, out var entity))
            {
                return;
            }

            this.ecb.Unlink(entity, obj);
        }

        public void AddComponent(Entity entity, IComponent component)
        {
            this.ecb.AddComponent(entity, component);
        }

        public void AddComponent<TComponent>(Entity entity, TComponent component) where TComponent : IComponent
        {
            this.ecb.AddComponent(entity, component);
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
            this.ecb.RemoveComponent(entity, type);
        }

        public void RemoveComponent<TComponent>(Entity entity) where TComponent : IComponent
        {
            this.ecb.RemoveComponent<TComponent>(entity);
        }

#endregion

#region Collector

        public Collector<T> RequireAll<T>()
            where T : IComponent
        {
            var collector = new Collector<T>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2> RequireAll<T1, T2>()
            where T1 : IComponent
            where T2 : IComponent
        {
            var collector = new Collector<T1, T2>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3> RequireAll<T1, T2, T3>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            var collector = new Collector<T1, T2, T3>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4> RequireAll<T1, T2, T3, T4>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4, T5> RequireAll<T1, T2, T3, T4, T5>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4, T5>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4, T5, T6> RequireAll<T1, T2, T3, T4, T5, T6>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4, T5, T6>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4, T5, T6, T7> RequireAll<T1, T2, T3, T4, T5, T6, T7>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4, T5, T6, T7>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4, T5, T6, T7, T8> RequireAll<T1, T2, T3, T4, T5, T6, T7, T8>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4, T5, T6, T7, T8>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4, T5, T6, T7, T8, T9> RequireAll<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
            where T9 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            this.collectors.Add(collector);

            return collector;
        }

        public Collector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> RequireAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
            where T6 : IComponent
            where T7 : IComponent
            where T8 : IComponent
            where T9 : IComponent
            where T10 : IComponent
        {
            var collector = new Collector<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
            this.collectors.Add(collector);

            return collector;
        }

#endregion
    }
}