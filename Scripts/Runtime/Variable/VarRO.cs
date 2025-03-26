namespace MK.Entities.Runtime.MK.Entities.Runtime.Variable
{
    public class VarRO<T>
    {
        protected T value;

        public T Get => this.value;

        public VarRO(T value)
        {
            this.value = value;
        }
    }
}