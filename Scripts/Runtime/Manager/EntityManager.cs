namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public sealed class EntityManager
    {
        private readonly Dictionary<ArchetypeKey, Archetype> keyToArchetype          = new();
        private readonly Dictionary<Entity, Archetype>       entityToArchetypeLookup = new();
        private readonly List<Command>                       commands                = new();
        private readonly Dictionary<Entity, GameObject>      entityToViewLinked      = new();

        private int entityCreationCount;

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
                        this.PlaybackCreate(command.InitialComponents);

                        break;
                    case CommandType.Destroy:
                        this.PlaybackDestroy(command.Entity);

                        break;
                    case CommandType.Add:
                        this.PlaybackAdd(command.Entity, command.NewComponent);

                        break;
                    case CommandType.Remove:
                        this.PlaybackRemove(command.Entity, command.ComponentType);

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

        private void PlaybackCreate(IComponent[] components)
        {
            if (components.Length == 0)
            {
                throw new InvalidOperationException($"Cannot create entity without any components.");
            }

            var entity       = new Entity(this.entityCreationCount++);
            var archetypeKey = new ArchetypeKey(components.Select(component => component.GetType()).ToArray());
            if (!this.keyToArchetype.TryGetValue(archetypeKey, out var archetype))
            {
                this.keyToArchetype[archetypeKey] = archetype = new Archetype(components.Select(component => component.GetType()).ToArray());
            }

            archetype.AddEntity(entity, components);
            this.entityToArchetypeLookup[entity] = archetype;
        }

        private void PlaybackDestroy(Entity entity)
        {
            if (!this.entityToArchetypeLookup.TryGetValue(entity, out var archetype))
            {
                throw new InvalidOperationException($"Cannot destroy entity: {entity} because it doesn't exist in any archetype.");
            }

            archetype.RemoveEntity(entity);
            this.entityToArchetypeLookup.Remove(entity);
        }

        private void PlaybackAdd(Entity entity, IComponent component)
        {
            if (!this.entityToArchetypeLookup.TryGetValue(entity, out var oldArchetype))
            {
                throw new InvalidOperationException($"Cannot add entity: {entity} because it doesn't exist in any archetype.");
            }

            var components = oldArchetype.GetComponents(entity).ToList();
            components.Add(component);
            oldArchetype.RemoveEntity(entity);

            var newArchetype = this.GetOrCreateArchetype(new ArchetypeKey(components.Select(comp => comp.GetType()).ToArray()));
            newArchetype.AddEntity(entity, components);
            this.entityToArchetypeLookup[entity] = newArchetype;
        }

        private void PlaybackRemove(Entity entity, Type componentType)
        {
            if (!this.entityToArchetypeLookup.TryGetValue(entity, out var oldArchetype))
            {
                throw new InvalidOperationException($"Cannot remove entity: {entity} because it doesn't exist in any archetype.");
            }

            var components       = oldArchetype.GetComponents(entity).ToList();
            var removedComponent = oldArchetype.GetComponent(entity, componentType);
            components.Remove(removedComponent);
            oldArchetype.RemoveEntity(entity);

            var newArchetype = this.GetOrCreateArchetype(new ArchetypeKey(components.Select(comp => comp.GetType()).ToArray()));
            newArchetype.AddEntity(entity, components);
            this.entityToArchetypeLookup[entity] = newArchetype;
        }

        private void PlaybackSet(Entity entity, IComponent component)
        {
            if (!this.entityToArchetypeLookup.TryGetValue(entity, out var archetype))
            {
                throw new InvalidOperationException($"Cannot set entity: {entity} because it doesn't exist in any archetype.");
            }

            archetype.SetComponent(entity, component);
        }

        private void PlaybackLink(Entity entity, GameObject view)
        {
            if (!this.entityToArchetypeLookup.ContainsKey(entity))
            {
                throw new InvalidOperationException($"Cannot link entity: {entity} because it doesn't exist in any archetype.");
            }

            if (this.entityToViewLinked.ContainsKey(entity))
            {
                throw new InvalidOperationException($"Cannot link entity: {entity} because it is already linked.");
            }

            var componentTypeToLinked = view.GetComponents<ILinked>().ToDictionary(linked => linked.ComponentType, linked => linked);
            foreach (var (_, linked) in componentTypeToLinked)
            {
                if (linked is ISerializeLinked serializeLinked)
                {
                    this.PlaybackAdd(entity, serializeLinked.CreateComponent());
                }
            }

            if (!this.entityToArchetypeLookup.TryGetValue(entity, out var archetype))
            {
                throw new InvalidOperationException($"Cannot find to link entity: {entity} because it doesn't exist in any archetype.");
            }

            var components = archetype.GetComponents(entity).ToList();
            foreach (var component in components)
            {
                if (componentTypeToLinked.TryGetValue(component.GetType(), out var linked))
                {
                    linked.OnLinked(component);
                }
            }

            this.entityToViewLinked[entity] = view;
        }

        private void PlaybackUnlink(Entity entity)
        {
            if (!this.entityToArchetypeLookup.ContainsKey(entity))
            {
                throw new InvalidOperationException($"Cannot unlink entity: {entity} because it doesn't exist in any archetype.");
            }

            if (!this.entityToViewLinked.TryGetValue(entity, out var view))
            {
                throw new InvalidOperationException($"Cannot unlink entity: {entity} because it doesn't exist in any view.");
            }

            var links = view.GetComponents<ILinked>();
            foreach (var linked in links)
            {
                linked.OnUnlinked();
            }

            this.entityToViewLinked.Remove(entity);
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