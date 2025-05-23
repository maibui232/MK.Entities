namespace MK.Entities
{
    public abstract class SerializeLinked<TComponent> : Linked<TComponent>, ISerializeLinked where TComponent : struct, IComponent
    {
        IComponent ISerializeLinked.CreateComponent()
        {
            ILinked linked = this;
            if (linked.Component != null)
            {
                return linked.Component;
            }

            var component = this.CreateComponent();
            ((ILinked)this).Component = component;

            return component;
        }

        protected abstract TComponent CreateComponent();
    }
}