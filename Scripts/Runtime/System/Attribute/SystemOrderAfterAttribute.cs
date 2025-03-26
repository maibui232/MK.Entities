namespace MK.Entities.Runtime
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SystemOrderAfterAttribute : Attribute
    {
        public Type SystemType { get; private set; }

        public SystemOrderAfterAttribute(Type systemType)
        {
            this.SystemType = systemType;
        }
    }
}