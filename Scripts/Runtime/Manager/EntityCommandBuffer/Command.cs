namespace MK.Entities
{
    internal readonly struct Command
    {
        public readonly Type        CommandType;
        public readonly Entity      Entity;
        public readonly IComponent  Component;
        public readonly System.Type ComponentType;

        public Command(Type commandType, Entity entity = default, IComponent component = null, System.Type componentType = null)
        {
            this.CommandType   = commandType;
            this.Entity        = entity;
            this.Component     = component;
            this.ComponentType = componentType;
        }

        public enum Type
        {
            Create,
            Destroy,
            Add,
            Remove,
            Set
        }
    }
}