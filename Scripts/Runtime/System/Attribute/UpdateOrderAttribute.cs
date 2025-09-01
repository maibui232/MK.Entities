namespace MK.Entities
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class UpdateOrderAttribute : Attribute
    {
        public UpdateOrder Order { get; }

        public UpdateOrderAttribute(UpdateOrder order)
        {
            this.Order = order;
        }
    }

    public enum UpdateOrder
    {
        PreUpdate = 0,
        Update = 1,
        FixedUpdate = 2,
        LateUpdate = 3
    }
}