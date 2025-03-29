namespace MK.Entities
{
    public interface ISerializeLinked : ILinked
    {
        IComponent CreateComponent();
    }
}