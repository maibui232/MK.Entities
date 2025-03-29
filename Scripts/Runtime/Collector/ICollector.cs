namespace MK.Entities
{
    using System.Collections.Generic;

    public interface ICollector
    {
        internal void Collect(IReadOnlyCollection<Entity> entities);
    }
}