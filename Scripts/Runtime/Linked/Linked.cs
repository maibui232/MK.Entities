namespace MK.Entities.Runtime
{
    using System;
    using UnityEngine;

    public abstract class Linked<TComponent> : MonoBehaviour, ILinked where TComponent : IComponent
    {
        private bool isLinked;

        IComponent ILinked.Component { get; set; }

        void ILinked.OnLinked(IComponent component)
        {
            if (this.isLinked)
            {
                throw new ArgumentException($"Could not link component {typeof(TComponent).FullName} again!");
            }

            this.isLinked = true;
            this.OnLinked((TComponent)component);
        }

        void ILinked.OnUnlinked(IComponent component)
        {
            if (!this.isLinked)
            {
                throw new ArgumentException($"Could not unlink component {typeof(TComponent).FullName}!");
            }

            this.isLinked = false;
            this.OnUnlinked((TComponent)component);
        }

        public abstract void OnLinked(TComponent component);

        public abstract void OnUnlinked(TComponent component);
    }
}