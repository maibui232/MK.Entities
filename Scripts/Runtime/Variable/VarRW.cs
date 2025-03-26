namespace MK.Entities.Runtime.MK.Entities.Runtime.Variable
{
    using System;

    public class VarRW<T> : VarRO<T>
    {
        public VarRW(T value) : base(value)
        {
        }

        public event Action<T> OnChange;

        public void Set(T v)
        {
            if (this.value.Equals(v))
            {
                return;
            }

            this.value = v;
            this.OnChange?.Invoke(this.value);
        }
    }
}