namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class EntityManager
    {
        private readonly Dictionary<ArchetypeKey, Archetype> keyToArchetype = new();
        private readonly List<Command>                       commands       = new();

#region Query

        private Archetype GetOrCreateArchetype(ArchetypeKey key)
        {
            if (this.keyToArchetype.TryGetValue(key, out var archetype)) return archetype;
            this.keyToArchetype[key] = archetype = new Archetype(key.ComponentTypes);

            return archetype;
        }

#endregion

#region Playback

        internal void PlaybackECB()
        {
            foreach (var command in this.commands)
            {
                switch (command.CommandType)
                {
                    case CommandType.Create:
                        this.PlaybackCreate();

                        break;
                    case CommandType.Destroy:
                        this.PlaybackDestroy(command.Entity);

                        break;
                    case CommandType.Add:
                        this.PlaybackAdd(command.Entity, command.NewComponent);

                        break;
                    case CommandType.Remove:
                        this.PlaybackRemove(command.Entity, command.CommandType);

                        break;
                    case CommandType.Set:
                        this.PlaybackSet(command.Entity, command.NewComponent);

                        break;
                    case CommandType.Link:
                        this.PlaybackLink(command.Entity, command.View);

                        break;
                    case CommandType.Unlink:
                        this.PlaybackUnlink(command.Entity);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            this.commands.Clear();
        }

        private void PlaybackCreate()
        {
            throw new NotImplementedException();
        }

        private void PlaybackDestroy(Entity commandEntity)
        {
            throw new NotImplementedException();
        }

        private void PlaybackAdd(Entity commandEntity, IComponent commandComponent)
        {
            throw new NotImplementedException();
        }

        private void PlaybackRemove(Entity commandEntity, CommandType commandCommandType)
        {
            throw new NotImplementedException();
        }

        private void PlaybackSet(Entity commandEntity, IComponent commandComponent)
        {
            throw new NotImplementedException();
        }

        private void PlaybackLink(Entity commandEntity, GameObject commandView)
        {
            throw new NotImplementedException();
        }

        private void PlaybackUnlink(Entity commandEntity)
        {
            throw new NotImplementedException();
        }

#endregion

#region Command Buffer

        public void CreateEntity(params IComponent[] components)
        {
            this.commands.Add(Command.Create());
        }

        public void DestroyEntity(Entity entity)
        {
            this.commands.Add(Command.Destroy(entity));
        }

        public void AddComponent(Entity entity, IComponent component)
        {
            this.commands.Add(Command.Add(entity, component));
        }

        public void RemoveComponent(Entity entity, Type componentType)
        {
            this.commands.Add(Command.Remove(entity, componentType));
        }

        public void RemoveComponent<T>(Entity entity) where T : struct, IComponent => this.RemoveComponent(entity, typeof(T));

        public void SetComponent(Entity entity, IComponent component)
        {
            this.commands.Add(Command.Set(entity, component));
        }

        public void LinkEntity(Entity entity, GameObject gameObject)
        {
            this.commands.Add(Command.Link(entity, gameObject));
        }

        public void UnlinkEntity(Entity entity)
        {
            this.commands.Add(Command.Unlink(entity));
        }

#endregion
    }
}