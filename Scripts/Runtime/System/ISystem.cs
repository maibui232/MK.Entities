namespace MK.Entities
{
    public interface ISystem
    {
        void OnCreate(World world);
        void OnUpdate(World world);
        void OnCleanUp(World world);
    }
}