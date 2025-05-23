namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class EntityCommandBuffer : IDisposable
    {
        private readonly List<ICommandBuffer> commandBuffers = new();

        public ICommandBuffer DestroyEntity(Entity entity, Action<Entity> onDestroyEntity)
        {
            var command = new DestroyCommandBuffer(entity, onDestroyEntity);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer AddComponent(Entity entity, IComponent component)
        {
            var command = new AddCommandBuffer(entity, component);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer SetComponent(Entity entity, IComponent component)
        {
            var command = new SetCommandBuffer(entity, component);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer RemoveComponent(Entity entity, Type type)
        {
            var command = new RemoveCommandBuffer(entity, type);
            this.commandBuffers.Add(command);

            return command;
        }

        internal ICommandBuffer RemoveComponent<TComponent>(Entity entity) where TComponent : IComponent
        {
            var command = new RemoveCommandBuffer(entity, typeof(TComponent));
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