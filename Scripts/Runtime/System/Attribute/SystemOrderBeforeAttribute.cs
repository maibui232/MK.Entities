namespace MK.Entities.Runtime
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SystemOrderBeforeAttribute : Attribute
    {
        public Type SystemType { get; private set; }

        public SystemOrderBeforeAttribute(Type systemType)
        {
            this.SystemType = systemType;
        }
    }
}