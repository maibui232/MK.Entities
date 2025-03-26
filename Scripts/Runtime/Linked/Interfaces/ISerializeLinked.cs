namespace MK.Entities.Runtime
{
    public interface ISerializeLinked : ILinked
    {
        IComponent CreateComponent();
    }
}