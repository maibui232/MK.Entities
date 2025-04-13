namespace MK.Entities
{
    public interface ILinked
    {
        internal IComponent Component { get; set; }
        internal bool       IsLinked  { get; }

        void OnLinked(IComponent component);

        void OnUnlinked(IComponent component);
    }
}