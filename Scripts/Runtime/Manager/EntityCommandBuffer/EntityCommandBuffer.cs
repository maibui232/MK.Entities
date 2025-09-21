namespace MK.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class EntityCommandBuffer
    {
        private readonly List<Command> commands = new();

        internal IReadOnlyCollection<Command> Commands => this.commands;

        internal void ClearBuffer()
        {
            this.commands.Clear();
        }

#region Commands

        internal void Create(params IComponent[] components)
        {
            this.commands.AddRange(components.Select(component => new Command(Command.Type.Create, component: component)));
        }

        internal void Destroy(Entity entity)
        {
            this.commands.Add(new Command(Command.Type.Destroy, entity));
        }

        internal void Add(Entity entity, params IComponent[] components)
        {
            this.commands.AddRange(components.Select(component => new Command(Command.Type.Add, entity, component)));
        }

        internal void Remove(Entity entity, params Type[] types)
        {
            this.commands.AddRange(types.Select(component => new Command(Command.Type.Remove, entity, componentType: component)));
        }

        internal void Set(Entity entity, params IComponent[] components)
        {
            this.commands.AddRange(components.Select(component => new Command(Command.Type.Set, entity, component)));
        }

#endregion
    }
}