namespace MK.Entities
{
    public class AddCommandBuffer : ICommandBuffer
    {
        private readonly Entity     entity;
        private readonly IComponent component;

        public AddCommandBuffer(Entity entity, IComponent component)
        {
            this.entity    = entity;
            this.component = component;
        }

        void ICommandBuffer.Execute()
        {
            this.entity.AddComponent(this.component);
        }
    }
}