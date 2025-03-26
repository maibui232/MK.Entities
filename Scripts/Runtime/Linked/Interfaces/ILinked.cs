namespace MK.Entities.Runtime
{
    public interface ILinked
    {
        internal IComponent Component { get; set; }

        void OnLinked(IComponent component);

        void OnUnlinked(IComponent component);
    }
}