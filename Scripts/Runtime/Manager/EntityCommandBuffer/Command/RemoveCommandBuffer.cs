namespace MK.Entities
{
    using System;

    internal sealed class RemoveCommandBuffer : ICommandBuffer
    {
        private readonly Entity entity;
        private readonly Type   type;
        private readonly Action onRemovedComponent;

        public RemoveCommandBuffer(Entity entity, Type type, Action onRemovedComponent)
        {
            this.entity             = entity;
            this.type               = type;
            this.onRemovedComponent = onRemovedComponent;
        }

        void ICommandBuffer.Execute()
        {
            this.entity.RemoveComponent(this.type);
            this.onRemovedComponent?.Invoke();
        }
    }
}