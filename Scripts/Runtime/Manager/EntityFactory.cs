namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using global::MK.Factory;

    public class EntityFactory : IFactory<Entity>
    {
        private readonly List<Entity> spawnedEntities = new();
        private readonly List<Entity> cachedEntities  = new();

        IEnumerable<Entity> IFactory<Entity>.GetSpawned => this.spawnedEntities;
        IEnumerable<Entity> IFactory<Entity>.GetCached => this.cachedEntities;

        Entity IFactory<Entity>.Instantiate()
        {
            Entity entity;
            if (this.cachedEntities.Count > 0)
            {
                entity = this.cachedEntities[0];
                this.cachedEntities.RemoveAt(0);
            }
            else
            {
                entity = new Entity();
            }

            this.spawnedEntities.Add(entity);

            return entity;
        }

        void IFactory<Entity>.Destruct(Entity instance)
        {
            if (!this.spawnedEntities.Remove(instance))
            {
                throw new ArgumentException("Entity instance is not spawned.");
            }

            this.cachedEntities.Add(instance);
        }

        void IFactory<Entity>.CleanUp()
        {
            this.spawnedEntities.Clear();
            this.cachedEntities.Clear();
        }
    }
}