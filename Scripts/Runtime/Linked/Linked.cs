namespace MK.Entities
{
    using System;
    using UnityEngine;

    public abstract class Linked<TComponent> : MonoBehaviour, ILinked where TComponent : struct, IComponent
    {
        private IComponent cacheComponent;
        private Type       componentType;

        IComponent ILinked.Component     => this.cacheComponent;
        bool ILinked.      IsLinked      => this.cacheComponent != null;
        Type ILinked.      ComponentType => typeof(TComponent);

        void ILinked.OnLinked(IComponent component)
        {
            if (this.cacheComponent != null)
            {
                throw new ArgumentException($"Could not link component {typeof(TComponent).FullName} again!");
            }

            this.cacheComponent = component;
            this.OnLinked((TComponent)component);
        }

        void ILinked.OnUnlinked()
        {
            if (this.cacheComponent == null)
            {
                throw new ArgumentException($"Could not unlink component {typeof(TComponent).FullName}!");
            }

            this.OnUnlinked((TComponent)this.cacheComponent);
            this.cacheComponent = null;
        }

        protected abstract void OnLinked(TComponent component);

        protected abstract void OnUnlinked(TComponent component);
    }
}