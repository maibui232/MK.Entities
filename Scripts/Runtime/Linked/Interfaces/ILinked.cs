namespace MK.Entities
{
    public interface ILinked
    {
        internal IComponent Component { get; set; }

        void OnLinked(IComponent component);

        void OnUnlinked(IComponent component);
    }
}