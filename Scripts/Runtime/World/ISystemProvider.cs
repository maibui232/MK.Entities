namespace MK.Entities.Runtime
{
    public interface ISystemProvider
    {
        TSystem CreateSystem<TSystem>() where TSystem : class, ISystem;
    }
}