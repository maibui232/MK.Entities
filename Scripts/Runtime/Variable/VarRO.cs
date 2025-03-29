namespace MK.Entities.MK.Entities.Variable
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