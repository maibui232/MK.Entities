namespace MK.Entities
{
    public abstract class SerializeLinked<TComponent> : Linked<TComponent>, ISerializeLinked where TComponent : struct, IComponent
    {
        IComponent ISerializeLinked.CreateComponent()
        {
            return this.CreateComponent();
        }

        protected abstract TComponent CreateComponent();
    }
}