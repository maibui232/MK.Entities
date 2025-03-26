namespace MK.Entities.Runtime
{
    using System;

    public class RemoveCommandBuffer : ICommandBuffer
    {
        private readonly Entity entity;
        private readonly Type   type;

        public RemoveCommandBuffer(Entity entity, Type type)
        {
            this.entity = entity;
            this.type   = type;
        }

        void ICommandBuffer.Execute()
        {
            this.entity.RemoveComponent(this.type);
        }
    }
}