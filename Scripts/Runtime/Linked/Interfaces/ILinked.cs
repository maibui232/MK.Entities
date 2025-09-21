namespace MK.Entities
{
    using System;

    public interface ILinked
    {
        internal IComponent Component     { get; }
        internal bool       IsLinked      { get; }
        internal Type       ComponentType { get; }

        void OnLinked(IComponent component);

        void OnUnlinked();
    }
}