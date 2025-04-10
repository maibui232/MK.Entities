namespace MK.Entities
{
    using UnityEngine;

    public interface ILinked
    {
        internal GameObject LinkedObject { get; }
        internal IComponent Component    { get; set; }

        void OnLinked(IComponent component);

        void OnUnlinked(IComponent component);
    }
}