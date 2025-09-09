namespace MK.Entities
{
    using MK.Pool;

    internal sealed class EntityPoolableController : BasePoolableController<Entity>
    {
        protected override Entity Create() => new Entity();
    }
}