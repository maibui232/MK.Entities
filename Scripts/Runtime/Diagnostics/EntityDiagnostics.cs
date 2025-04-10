#if UNITY_EDITOR
namespace MK.Entities
{
    using System;
    using System.Collections.Generic;

    public class EntityDiagnostics
    {
        public EntityDiagnostics(Entity entity) { this.Entity = entity; }

        public Entity                  Entity     { get; }
        public IEnumerable<IComponent> Components => this.Entity.Components;
        public string                  Name       => this.Entity.Name;
        public int                     Index      => this.Entity.Index;

        public bool HasComponent(Type componentType) => this.Entity.HasComponent(componentType);
    }
}
#endif