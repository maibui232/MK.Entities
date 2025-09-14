namespace MK.Entities
{
    using System;

    internal sealed class AddCommandBuffer : ICommandBuffer
    {
        private readonly Entity     entity;
        private readonly IComponent component;
        private readonly Action     onAddedComponent;

        public AddCommandBuffer(Entity entity, IComponent component, Action onAddedComponent)
        {
            this.entity           = entity;
            this.component        = component;
            this.onAddedComponent = onAddedComponent;
        }

        void ICommandBuffer.Execute()
        {
            this.entity.AddComponent(this.component);
            this.onAddedComponent?.Invoke();
        }
    }
}