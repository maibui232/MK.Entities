namespace MK.Entities
{
    using UnityEngine;

    public class UnlinkCommandBuffer : ICommandBuffer
    {
        private readonly Entity     entity;
        private readonly GameObject gameObject;

        public UnlinkCommandBuffer(Entity entity, GameObject gameObject)
        {
            this.entity     = entity;
            this.gameObject = gameObject;
        }

        public void Execute()
        {
            var links = this.gameObject.GetComponents<ILinked>();
            foreach (var link in links)
            {
                var componentType = link.GetType().BaseType?.GetGenericArguments()[0];
                link.OnUnlinked(this.entity.GetComponent(componentType));
            }
        }
    }
}