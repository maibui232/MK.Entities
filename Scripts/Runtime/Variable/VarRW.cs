namespace MK.Entities
{
    using System;

    public class VarRW<T> : VarRO<T>
    {
        public VarRW(T value) : base(value) { }

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

        public static implicit operator T(VarRW<T> v) { return v.value; }

        public static implicit operator VarRW<T>(T v) { return new VarRW<T>(v); }

        public override string ToString() { return this.value.ToString(); }
    }
}