namespace MK.Entities
{
    using System;
    using System.Linq;
    using UnityEngine;

    public class LinkCommandBuffer : ICommandBuffer
    {
        private readonly Entity     entity;
        private readonly GameObject gameObject;

        public LinkCommandBuffer(Entity entity, GameObject gameObject)
        {
            this.entity     = entity;
            this.gameObject = gameObject;
        }

        public void Execute()
        {
            var links = this.gameObject.GetComponents<ILinked>();
            foreach (var link in links.Where(link => !link.IsLinked))
            {
                var  linkType = link.GetType();
                Type componentType;

                if (link is ISerializeLinked serializeLinked)
                {
                    var newComponent = serializeLinked.CreateComponent();
                    this.entity.AddComponent(newComponent);
                    componentType = newComponent.GetType();
                }
                else
                {
                    componentType = linkType.BaseType?.GetGenericArguments()[0];
                }

                if (!this.entity.HasComponent(componentType))
                {
                    throw new ArgumentException($"{this.entity.Name} could not be found component {componentType?.FullName}");
                }

                link.OnLinked(this.entity.GetComponent(componentType));
            }
        }
    }
}