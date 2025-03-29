namespace MK.Entities
{
    using System;
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
            foreach (var link in links)
            {
                var linkType      = link.GetType();
                var componentType = linkType.BaseType?.GetGenericArguments()[0];

                if (link is ISerializeLinked serializeLinked)
                {
                    var newComponent = serializeLinked.CreateComponent();
                    this.entity.AddComponent(newComponent);
                }

                if (!this.entity.HasComponent(componentType))
                {
                    throw new ArgumentException($"Entity-{this.entity.Name} could not be found component {componentType?.FullName}");
                }

                link.OnLinked(this.entity.GetComponent(componentType));
            }
        }
    }
}