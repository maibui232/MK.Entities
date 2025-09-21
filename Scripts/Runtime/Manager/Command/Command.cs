namespace MK.Entities
{
    using System;
    using UnityEngine;

    internal readonly struct Command
    {
        internal readonly CommandType  CommandType;
        internal readonly Entity       Entity;
        internal readonly IComponent[] InitialComponents;
        internal readonly IComponent   NewComponent;
        internal readonly Type         ComponentType;
        internal readonly GameObject   View;

        private Command(CommandType commandType, Entity entity = default, IComponent[] initialComponents = null, IComponent newComponent = null, Type componentType = null, GameObject view = null)
        {
            this.CommandType       = commandType;
            this.Entity            = entity;
            this.InitialComponents = initialComponents;
            this.NewComponent      = newComponent;
            this.ComponentType     = componentType;
            this.View              = view;
        }

        public static Command Create(params IComponent[] initialComponents)
        {
            return new Command(CommandType.Create, initialComponents: initialComponents);
        }

        public static Command Destroy(Entity entity)
        {
            return new Command(CommandType.Destroy, entity);
        }

        public static Command Add(Entity entity, IComponent component)
        {
            return new Command(CommandType.Add, entity, newComponent: component);
        }

        public static Command Remove(Entity entity, Type componentType)
        {
            return new Command(CommandType.Remove, entity, componentType: componentType);
        }

        public static Command Set(Entity entity, IComponent component)
        {
            return new Command(CommandType.Set, entity, newComponent: component);
        }

        public static Command Link(Entity entity, GameObject view)
        {
            return new Command(CommandType.Link, entity, view: view);
        }

        public static Command Unlink(Entity entity)
        {
            return new Command(CommandType.Unlink, entity);
        }
    }
}