namespace MK.Entities
{
    using System;
    using MK.Pool;

    internal sealed class DestroyCommandBuffer : ICommandBuffer
    {
        private readonly Entity                            entity;
        private readonly IObjectPool<Entity, Entity.Param> objectPool;
        private readonly Action                            onDestroyed;

        public DestroyCommandBuffer(Entity entity, IObjectPool<Entity, Entity.Param> objectPool, Action onDestroyed)
        {
            this.entity      = entity;
            this.objectPool  = objectPool;
            this.onDestroyed = onDestroyed;
        }

        void ICommandBuffer.Execute()
        {
            this.objectPool.Recycle(this.entity);
            this.onDestroyed?.Invoke();
        }
    }
}