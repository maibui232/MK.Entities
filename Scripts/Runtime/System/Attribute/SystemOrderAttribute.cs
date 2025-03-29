namespace MK.Entities
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SystemOrderAttribute : Attribute
    {
        public int Order { get; private set; }

        public SystemOrderAttribute(int order)
        {
            this.Order = order;
        }
    }
}