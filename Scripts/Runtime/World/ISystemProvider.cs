namespace MK.Entities
{
    public interface ISystemProvider
    {
        TSystem CreateSystem<TSystem>() where TSystem : class, ISystem;
    }
}