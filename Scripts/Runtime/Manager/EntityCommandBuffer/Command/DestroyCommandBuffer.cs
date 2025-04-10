namespace MK.Entities
{
    using System;

    public class DestroyCommandBuffer : ICommandBuffer
    {
        private readonly Entity         entity;
        private readonly Action<Entity> onDestroyEntity;

        public DestroyCommandBuffer(Entity entity, Action<Entity> onDestroyEntity)
        {
            this.entity          = entity;
            this.onDestroyEntity = onDestroyEntity;
        }

        public void Execute()
        {
            this.entity.OnDestroy();
            this.onDestroyEntity?.Invoke(this.entity);
        }
    }
}