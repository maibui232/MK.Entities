namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using MK.Pool;
    using UnityEngine;

    internal sealed class EntityCommandBuffer : IDisposable
    {
        private readonly List<ICommandBuffer> commandBuffers = new();

        public ICommandBuffer DestroyEntity(Entity entity, IObjectPool<Entity, Entity.Param> objectPool, Action onDestroyed)
        {
            var command = new DestroyCommandBuffer(entity, objectPool, onDestroyed);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer AddComponent(Entity entity, IComponent component, Action onAddedComponent)
        {
            var command = new AddCommandBuffer(entity, component, onAddedComponent);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer SetComponent(Entity entity, IComponent component)
        {
            var command = new SetCommandBuffer(entity, component);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer RemoveComponent(Entity entity, Type type, Action onRemovedComponent)
        {
            var command = new RemoveCommandBuffer(entity, type, onRemovedComponent);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer Link(Entity entity, GameObject gameObject)
        {
            var command = new LinkCommandBuffer(entity, gameObject);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer Unlink(GameObject gameObject)
        {
            var command = new UnlinkCommandBuffer(gameObject);
            this.commandBuffers.Add(command);

            return command;
        }

        internal void Playback()
        {
            foreach (var commandBuffer in this.commandBuffers)
            {
                commandBuffer.Execute();
            }

            this.commandBuffers.Clear();
        }

        void IDisposable.Dispose() { this.commandBuffers.Clear(); }
    }
}