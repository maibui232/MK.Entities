namespace MK.Entities
{
    using System;
    using UnityEngine;

    public abstract class Linked<TComponent> : MonoBehaviour, ILinked where TComponent : struct, IComponent
    {
        private bool       isLinked;
        private GameObject linkedObject;

        IComponent ILinked.Component { get; set; }
        bool ILinked.      IsLinked  => this.isLinked;

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

        protected abstract void OnLinked(TComponent component);

        protected abstract void OnUnlinked(TComponent component);
    }
}