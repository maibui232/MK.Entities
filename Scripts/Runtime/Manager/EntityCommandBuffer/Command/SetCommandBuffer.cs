namespace MK.Entities
{
    public class SetCommandBuffer : ICommandBuffer
    {
        private readonly Entity     entity;
        private readonly IComponent component;

        public SetCommandBuffer(Entity entity, IComponent component)
        {
            this.entity    = entity;
            this.component = component;
        }

        void ICommandBuffer.Execute()
        {
            this.entity.SetComponent(this.component);
        }
    }
}