namespace MK.Entities
{
    using System;
    using System.Collections.Generic;

    public sealed class EntityManager
    {
        private readonly Dictionary<ArchetypeKey, Archetype> keyToArchetype      = new();
        private readonly EntityCommandBuffer                 entityCommandBuffer = new();

#region Query

        private bool GetOrCreateArchetype(ArchetypeKey key, out Archetype archetype)
        {
            if (this.keyToArchetype.TryGetValue(key, out archetype)) return true;
            this.keyToArchetype[key] = archetype = new Archetype(key.ComponentTypes);

            return false;
        }

#endregion

#region Playback

        internal void PlaybackECB()
        {
            foreach (var command in this.entityCommandBuffer.Commands)
            {
                switch (command.CommandType)
                {
                    case Command.Type.Create:
                        this.PlaybackCreate(command);

                        break;
                    case Command.Type.Destroy:
                        this.PlaybackDestroy(command);

                        break;
                    case Command.Type.Add:
                        this.PlaybackAdd(command);

                        break;
                    case Command.Type.Remove:
                        this.PlaybackRemove(command);

                        break;
                    case Command.Type.Set:
                        this.PlaybackSet(command);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void PlaybackCreate(Command command)
        {
        }

        private void PlaybackDestroy(Command command)
        {
        }

        private void PlaybackAdd(Command command)
        {
        }

        private void PlaybackRemove(Command command)
        {
        }

        private void PlaybackSet(Command command)
        {
        }

#endregion

#region Command Buffer

        public void CreateEntity(params IComponent[] components)
        {
            this.entityCommandBuffer.Create(components);
        }

        public void DestroyEntity(Entity entity)
        {
            this.entityCommandBuffer.Destroy(entity);
        }

        public void AddComponent(Entity entity, IComponent component)
        {
            this.entityCommandBuffer.Add(entity, component);
        }

        public void AddComponent<TComponent>(Entity entity, TComponent component) where TComponent : struct, IComponent
        {
            this.entityCommandBuffer.Add(entity, component);
        }

        public void RemoveComponent(Entity entity, Type type)
        {
            this.entityCommandBuffer.Remove(entity, type);
        }

        public void RemoveComponent<TComponent>(Entity entity) where TComponent : struct, IComponent
        {
            this.entityCommandBuffer.Remove(entity, typeof(TComponent));
        }

        public void SetComponent(Entity entity, IComponent component)
        {
            this.entityCommandBuffer.Set(entity, component);
        }

        public void SetComponent<TComponent>(Entity entity, TComponent component) where TComponent : struct, IComponent
        {
            this.entityCommandBuffer.Set(entity, component);
        }

#endregion
    }
}