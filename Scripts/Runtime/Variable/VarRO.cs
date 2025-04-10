namespace MK.Entities
{
    public class VarRO<T>
    {
        protected T value;

        public T Get => this.value;

        public VarRO(T value)
        {
            this.value = value;
        }

        public static implicit operator T(VarRO<T> v) { return v.value; }

        public static implicit operator VarRO<T>(T v) { return new VarRO<T>(v); }

        public override string ToString() { return this.value.ToString(); }
    }
}