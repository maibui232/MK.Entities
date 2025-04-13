namespace MK.Entities
{
    using System.Linq;
    using UnityEngine;

    public class UnlinkCommandBuffer : ICommandBuffer
    {
        private readonly Entity     entity;
        private readonly GameObject gameObject;

        public UnlinkCommandBuffer(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Execute()
        {
            var links = this.gameObject.GetComponents<ILinked>();
            foreach (var link in links.Where(link => link.IsLinked))
            {
                var componentType = link.GetType().BaseType?.GetGenericArguments()[0];
                link.OnUnlinked(this.entity.GetComponent(componentType));
            }
        }
    }
}